using System.Diagnostics.CodeAnalysis;
using MaaFramework.Binding.Abstractions;
using MaaFramework.Binding.Buffers;

namespace MaaFramework.Binding.UnitTests;

/// <summary>
///     Test namespace <see cref="MaaFramework.Binding.Buffers"/>.
/// </summary>
[TestClass]
// ReSharper disable once InconsistentNaming
public class Test_Buffers
{
    public static Dictionary<MaaTypes, object[]> NewData => new()
    {
#if MAA_NATIVE
        {
            MaaTypes.Native, [
                MaaImage.Load<MaaImageBuffer>(Common.ImagePath),
                new MaaImageBuffer(),
                new MaaRectBuffer(),
                new MaaStringBuffer(),
                new MaaImageListBuffer(),
                new MaaStringListBuffer(),
                (AdbDeviceListBuffer)MaaToolkit.Shared.AdbDevice.Find(),
#if MAA_WIN32
                (DesktopWindowListBuffer)MaaToolkit.Shared.Desktop.Window.Find(),
#endif
            ]
        },
#endif
    };

    private static MaaImage GetNewImage(MaaTypes type) => type switch
    {
#if MAA_NATIVE
        MaaTypes.Native => MaaImage.Load<MaaImageBuffer>(Common.ImagePath),
#endif
        _ => throw new NotImplementedException(),
    };

    public static Dictionary<MaaTypes, IMaaStringBuffer> NewStrings
    {
        get
        {
            return new Dictionary<MaaTypes, IMaaStringBuffer>
            {
#if MAA_NATIVE
                {
                    MaaTypes.Native, GetStringBuffer(MaaTypes.Native)
                },
#endif
            };

            static IMaaStringBuffer GetStringBuffer(MaaTypes type)
            {
                MaaStringBuffer? buffer;
                var testString = type.ToString() + '\0' + type.ToString();

                if (type == MaaTypes.Native)
                    buffer = new MaaStringBuffer();
                else
                    throw new NotImplementedException();

                Assert.IsTrue(
                    buffer.TrySetValue(testString));
                return buffer;
            }
        }
    }

    public static Dictionary<MaaTypes, object[]> Data { get; private set; } = default!;

    [ClassInitialize]
    public static void InitializeClass(TestContext context)
    {
        Assert.IsTrue(File.Exists(Common.ImagePath));

        Data = NewData;
    }

    [ClassCleanup]
    public static void CleanUpClass()
        => Common.DisposeData(Data.Values.SelectMany(objs => objs).Cast<IMaaDisposable>());

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(GetNewImage))]
    public void Test_MaaImage_NoCache(MaaTypes type, MaaImage image)
    {
        Assert.IsNotNull(image);
        Assert.IsNotNull(image.Buffer);

        Assert.AreEqual(
            "MaaImage: 1920x1080 { Channels = 3, Type = 16 }", image.ToString());
        Assert.AreEqual(
            new ImageInfo(Width: 1920, Height: 1080, Channels: 3, Type: 16), image.GetInfo());

        image.ThrowOnInvalid = true;
        Assert.AreEqual(
            image.ThrowOnInvalid, image.Buffer.ThrowOnInvalid);

        Assert.IsFalse(
            image.IsInvalid);
        image.Dispose();
        Assert.IsTrue(
            image.IsInvalid);
        _ = Assert.ThrowsException<ObjectDisposedException>(() =>
            image.TryCache());
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(GetNewImage))]
    public void Test_MaaImage_Cache(MaaTypes type, MaaImage image)
    {
        Assert.IsNotNull(image);
        Assert.IsNotNull(image.Buffer);

        Assert.IsTrue(
            image.TryCache());
        image.ThrowOnInvalid = true;
        image.Dispose();
        Assert.IsFalse(
            image.IsInvalid);

        Assert.AreEqual(
            image.ThrowOnInvalid, image.Buffer.ThrowOnInvalid);
        Assert.AreEqual(
            "MaaImage: 1920x1080 { Channels = 3, Type = 16 }", image.ToString());
        Assert.AreEqual(
            new ImageInfo(Width: 1920, Height: 1080, Channels: 3, Type: 16), image.GetInfo());
        _ = Assert.ThrowsException<ObjectDisposedException>(() =>
            image.Buffer.IsEmpty);
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data))]
    public void Test_IMaaImageBuffer(MaaTypes type, IMaaImageBuffer buffer)
    {
        Assert.IsNotNull(buffer);
        var native = (buffer as MaaImageBuffer)!;
        if (type == MaaTypes.Native)
        {
            Assert.IsNotNull(native);
            native = new MaaImageBuffer(native.Handle);
        }

        # region No data

        Assert.IsTrue(
            buffer.IsEmpty);
        Assert.IsFalse(
            buffer.IsInvalid);
        Assert.AreEqual(
            0, buffer.Width);
        Assert.AreEqual(
            0, buffer.Height);
        Assert.AreEqual(
            1, buffer.Channels);
        Assert.AreEqual(
            0, buffer.Type);
        Assert.AreEqual(
            new ImageInfo(Width: 0, Height: 0, Channels: 1, Type: 0), buffer.GetInfo());
        Assert.IsFalse(
            buffer.TryCopyTo(buffer));

        Assert.IsFalse(
            buffer.TryGetEncodedData(out byte[]? encodedDataArray));
        Assert.IsNull(
            encodedDataArray);
        Assert.IsFalse(
            buffer.TrySetEncodedData(encodedDataArray!));

        Assert.IsFalse(
            buffer.TryGetEncodedData(out Stream? encodedDataStream));
        Assert.IsNull(
            encodedDataStream);
        Assert.IsFalse(
            buffer.TrySetEncodedData(encodedDataStream!));

        Assert.IsFalse(
            buffer.TryGetEncodedData(out ReadOnlySpan<byte> encodedDataSpan));
        Assert.AreEqual(
            0, encodedDataSpan.Length);
        Assert.IsFalse(
            buffer.TrySetEncodedData(encodedDataSpan));

        if (type == MaaTypes.Native)
        {
            Assert.AreEqual(
                "MaaImageBuffer: 0x0 { Channels = 1, Type = 0 }", native.ToString());

            Assert.IsFalse(
                native.TryGetRawData(out var rawData));
            Assert.AreEqual(
                nint.Zero, rawData);
            Assert.IsFalse(
                MaaImageBuffer.TrySetRawData(native.Handle, rawData, native.Width, native.Height, native.Type));
        }
        Assert.IsTrue(
            buffer.TryClear());

        #endregion

        using var img = GetNewImage(type);
        Assert.IsTrue(
            img.Buffer.TryCopyTo(buffer));
        Assert.IsTrue(
            img.TryCache());

        #region Have data empty_1920x1080.png

        Assert.IsFalse(
            buffer.IsEmpty);
        Assert.IsFalse(
            buffer.IsInvalid);
        Assert.AreEqual(
            1920, buffer.Width);
        Assert.AreEqual(
            1080, buffer.Height);
        Assert.AreEqual(
            3, buffer.Channels);
        Assert.AreEqual(
            16, buffer.Type);
        Assert.AreEqual(
            new ImageInfo(Width: 1920, Height: 1080, Channels: 3, Type: 16), buffer.GetInfo());
        Assert.IsTrue(
            buffer.TryCopyTo(buffer));

        Assert.IsTrue(
            buffer.TryGetEncodedData(out encodedDataArray));
        Assert.IsNotNull(
            encodedDataArray);
        Assert.IsTrue(
            buffer.TrySetEncodedData(encodedDataArray));

        Assert.IsTrue(
            buffer.TryGetEncodedData(out encodedDataStream));
        Assert.IsNotNull(
            encodedDataStream);
        Assert.IsTrue(
            buffer.TrySetEncodedData(encodedDataStream));

        Assert.IsTrue(
            buffer.TryGetEncodedData(out encodedDataSpan));
        Assert.AreNotEqual(
            0, encodedDataSpan.Length);
        Assert.IsTrue(
            buffer.TrySetEncodedData(encodedDataSpan));

        if (type == MaaTypes.Native)
        {
            Assert.AreEqual(
                "MaaImageBuffer: 1920x1080 { Channels = 3, Type = 16 }", native.ToString());

            Assert.IsTrue(
                native.TryGetRawData(out var rawData));
            Assert.AreNotEqual(
                nint.Zero, rawData);
            Assert.IsTrue(
                native.TrySetRawData(rawData, native.Width, native.Height, native.Type));
        }

        #endregion

        #region Func & Handle

        if (type == MaaTypes.Native)
        {
            Assert.IsTrue(
                MaaImageBuffer.TryGetEncodedData(out var funcArray, handle
                    => MaaImageBuffer.TrySetEncodedData(handle, encodedDataArray)));
            CollectionAssert.AreEqual(
                encodedDataArray, funcArray);
            Assert.IsTrue(
                MaaImageBuffer.TrySetEncodedData(encodedDataArray, handle
                    => MaaImageBuffer.TryGetEncodedData(handle, out funcArray)));
            CollectionAssert.AreEqual(
                encodedDataArray, funcArray);

            Assert.IsTrue(img.Buffer.TryGetEncodedData(out encodedDataStream)
                          && img.Buffer.TryGetEncodedData(out encodedDataSpan));
            Assert.IsTrue(
                MaaImageBuffer.TrySetEncodedData(encodedDataStream, handle
                    => MaaImageBuffer.TryGetEncodedData(handle, out funcArray)));
            CollectionAssert.AreEqual(
                encodedDataArray, funcArray);
            Assert.IsTrue(
                MaaImageBuffer.TrySetEncodedData(encodedDataSpan, handle
                    => MaaImageBuffer.TryGetEncodedData(handle, out funcArray)));
            CollectionAssert.AreEqual(
                encodedDataArray, funcArray);

            Assert.IsTrue(
                MaaImageBuffer.TryGetEncodedData(native.Handle, out var encodedDataHandle, out var encodedDataSize));
            Assert.IsTrue(
                MaaImageBuffer.TrySetEncodedData(native.Handle, encodedDataHandle, encodedDataSize));
            Assert.IsTrue(
                MaaImageBuffer.TryGetRawData(native.Handle, out var rawDataHandle, out var rawWidth, out var rawHeight, out var rawType));
            Assert.IsTrue(
                MaaImageBuffer.TrySetRawData(native.Handle, rawDataHandle, rawWidth, rawHeight, rawType));
        }

        #endregion

        Assert.IsTrue(
            buffer.TryClear());
        Assert.IsTrue(
            buffer.IsEmpty);

        #region Invalid Data

        Assert.IsFalse(
            buffer.TryCopyTo(null!));
        _ = Assert.ThrowsException<NotImplementedException>(() =>
            buffer.TryCopyTo(new TestImageBuffer()));

        if (type == MaaTypes.Native)
        {
            Assert.IsTrue( // should be false
                MaaImageBuffer.TryGetEncodedData(native.Handle, out var invalidEncodedDataHandle, out var invalidEncodedDataSize));
            Assert.IsFalse(
                MaaImageBuffer.TrySetEncodedData(native.Handle, nint.Zero, invalidEncodedDataSize));
            Assert.IsFalse(
                MaaImageBuffer.TryGetRawData(native.Handle, out var invalidRawDataHandle, out _, out _, out _));
            Assert.IsFalse(
                MaaImageBuffer.TrySetRawData(native.Handle, nint.Zero, 0, 0, 0));
            Assert.AreEqual(
                nint.Zero, invalidRawDataHandle);
            // Assert.AreEqual(
            //     nint.Zero, invalidEncodedDataHandle);
            Assert.AreEqual(
                ulong.MinValue, invalidEncodedDataSize);

            Assert.IsFalse(
                MaaImageBuffer.TrySetEncodedData(encodedDataArray, static _ => false));
            Assert.IsFalse(
                MaaImageBuffer.TrySetEncodedData(encodedDataStream, static _ => false));
            Assert.IsFalse(
                MaaImageBuffer.TrySetEncodedData(encodedDataSpan, static _ => false));
            Assert.IsFalse(
                MaaImageBuffer.TryGetEncodedData(out encodedDataArray, static _ => false));
            Assert.IsNull(
                encodedDataArray);
        }

        #endregion

    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data))]
    public void Test_IMaaStringBuffer(MaaTypes type, IMaaStringBuffer buffer)
    {
        Assert.IsNotNull(buffer);
        var native = (buffer as MaaStringBuffer)!;
        if (type == MaaTypes.Native)
        {
            Assert.IsNotNull(native);
            native = new MaaStringBuffer(native.Handle);
        }

        # region No data

        Assert.IsTrue(
            buffer.IsEmpty);
        Assert.AreEqual<ulong>(
            0, buffer.Size);
        Assert.IsTrue( // Copy string.Empty
            buffer.TryCopyTo(buffer));

        Assert.AreEqual(
            string.Empty, buffer.ToString());
        Assert.IsTrue(
            buffer.TryGetValue(out var str));
        Assert.AreEqual(
            string.Empty, str);
        Assert.IsTrue(
            buffer.TrySetValue(str));
        if (type == MaaTypes.Native)
        {
            Assert.IsTrue(
                MaaStringBuffer.TryGetValue(native.Handle, out str));
            Assert.AreEqual(
                string.Empty, str);
            Assert.IsTrue(
                MaaStringBuffer.TrySetValue(native.Handle, str));
        }

        Assert.IsTrue(
            buffer.TryClear());

        #endregion

        var testString = type.ToString() + '\0' + type.ToString();
        Assert.IsTrue(
            buffer.TrySetValue(testString, true));

        #region Have data

        Assert.IsFalse(
            buffer.IsEmpty);
        Assert.AreEqual(
            (ulong)testString.Length, buffer.Size);
        Assert.IsTrue(
            buffer.TryCopyTo(buffer));

        Assert.AreEqual(
            testString, buffer.ToString());
        Assert.IsTrue(
            buffer.TryGetValue(out str));
        Assert.AreEqual(
            testString, str);
        if (type == MaaTypes.Native)
        {
            Assert.IsTrue(
                MaaStringBuffer.TrySetValue(native.Handle, testString, false));
            Assert.AreNotEqual( // \0 cut off if not useEx
                (ulong)testString.Length, buffer.Size);
            Assert.IsTrue(
                MaaStringBuffer.TryGetValue(native.Handle, out str));
            Assert.AreEqual(
                type.ToString(), str);
        }

        #endregion

        #region Func & Handle

        if (type == MaaTypes.Native)
        {
            Assert.IsTrue(
                MaaStringBuffer.TryGetValue(out var funcString, handle
                    => MaaStringBuffer.TrySetValue(handle, testString)));
            Assert.AreEqual(
                testString, funcString);
            Assert.IsTrue(
                MaaStringBuffer.TrySetValue(testString, true, handle
                    => MaaStringBuffer.TryGetValue(handle, out funcString)));
            Assert.AreEqual(
                testString, funcString);
        }

        #endregion

        Assert.IsTrue(
            buffer.TryClear());
        Assert.IsTrue(
            buffer.IsEmpty);

        #region Invalid Data

        Assert.IsFalse(
            buffer.TryCopyTo(null!));
        _ = Assert.ThrowsException<NotImplementedException>(() =>
            buffer.TryCopyTo(new TestStringBuffer()));

        if (type == MaaTypes.Native)
        {
            Assert.IsTrue(
                MaaStringBuffer.TryGetValue(native.Handle, out var invalidString));
            Assert.AreEqual(
                string.Empty, invalidString);
            Assert.IsFalse(
                MaaStringBuffer.TryGetValue(nint.Zero, out invalidString));
            Assert.IsNull(
                invalidString);

            Assert.IsFalse(
                MaaStringBuffer.TrySetValue(testString, true, static _ => false));
            Assert.IsFalse(
                MaaStringBuffer.TryGetValue(out invalidString, static _ => false));
            Assert.IsNull(
                invalidString);
        }

        #endregion

    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data))]
    public void Test_IMaaRectBuffer(MaaTypes type, IMaaRectBuffer buffer)
    {
        Assert.IsNotNull(buffer);
        var native = (buffer as MaaRectBuffer)!;
        if (type == MaaTypes.Native)
        {
            Assert.IsNotNull(native);
            native = new MaaRectBuffer(native.Handle);
        }

        # region No data

        Assert.AreEqual(
            0, buffer.X);
        Assert.AreEqual(
            0, buffer.Y);
        Assert.AreEqual(
            0, buffer.Width);
        Assert.AreEqual(
            0, buffer.Height);

        Assert.IsTrue( // Copy (0, 0, 0, 0)
            buffer.TryCopyTo(buffer));
        Assert.IsTrue(
            buffer.TryGetValues(out var x, out var y, out var width, out var height));
        Assert.IsTrue(
            buffer.TrySetValues(x, y, width, height));
        if (type == MaaTypes.Native)
        {
            Assert.AreEqual(
                "MaaRectBuffer { X = 0, Y = 0, Width = 0, Height = 0 }", buffer.ToString());

            Assert.IsTrue(
                MaaRectBuffer.TryGetValues(native.Handle, out x, out y, out width, out height));
            Assert.IsTrue(
                MaaRectBuffer.TrySetValues(native.Handle, x, y, width, height));
        }

        #endregion

        Assert.IsTrue(
            buffer.TrySetValues(1, 2, 3, 4));

        #region Have data

        CollectionAssert.AreEqual(
            (int[])[1, 2, 3, 4],
            (int[])[buffer.X, buffer.Y, buffer.Width, buffer.Height]);
        Assert.IsTrue(
            buffer.TryCopyTo(buffer));
        Assert.IsTrue(
            buffer.TryGetValues(out x, out y, out width, out height));
        Assert.IsTrue(
            buffer.TrySetValues(x, y, width, height));
        if (type == MaaTypes.Native)
        {
            Assert.AreEqual(
                "MaaRectBuffer { X = 1, Y = 2, Width = 3, Height = 4 }", buffer.ToString());

            Assert.IsTrue(
                MaaRectBuffer.TryGetValues(native.Handle, out x, out y, out width, out height));
            Assert.IsTrue(
                MaaRectBuffer.TrySetValues(native.Handle, x, y, width, height));
        }

        #endregion

        #region Func & Handle

        if (type == MaaTypes.Native)
        {
            Assert.IsTrue(
                MaaRectBuffer.TryGetValues(out var funcX, out var funcY, out var funcWidth, out var funcHeight, handle
                    => MaaRectBuffer.TrySetValues(handle, 1, 2, 3, 4)));
            CollectionAssert.AreEqual(
                (int[])[1, 2, 3, 4],
                (int[])[funcX, funcY, funcWidth, funcHeight]);

            Assert.IsTrue(
                MaaRectBuffer.TrySetValues(1, 2, 3, 4, handle
                    => MaaRectBuffer.TryGetValues(handle, out funcX, out funcY, out funcWidth, out funcHeight)));
            CollectionAssert.AreEqual(
                (int[])[1, 2, 3, 4],
                (int[])[funcX, funcY, funcWidth, funcHeight]);
        }

        #endregion

        #region Invalid Data

        Assert.IsFalse(
            buffer.TryCopyTo(null!));
        _ = Assert.ThrowsException<NotImplementedException>(() =>
            buffer.TryCopyTo(new TestRectBuffer()));

        if (type == MaaTypes.Native)
        {
            Assert.IsFalse(
                MaaRectBuffer.TryGetValues(nint.Zero, out var invalidX, out var invalidY, out var invalidWidth, out var invalidHeight));
            CollectionAssert.AreEqual(
                (int[])[0, 0, 0, 0],
                (int[])[invalidX, invalidY, invalidWidth, invalidHeight]);
            Assert.IsFalse(
                MaaRectBuffer.TrySetValues(nint.Zero, invalidX, invalidY, invalidWidth, invalidHeight));

            Assert.IsFalse(
                MaaRectBuffer.TrySetValues(invalidX, invalidY, invalidWidth, invalidHeight, static _ => false));
            Assert.IsFalse(
                MaaRectBuffer.TryGetValues(out invalidX, out invalidY, out invalidWidth, out invalidHeight, static _ => false));
        }

        #endregion

    }

    [TestMethod]
    [MaaData(MaaTypes.Native, nameof(Data))]
    public void Test_MaaImageListBuffer(MaaTypes type, MaaImageListBuffer buffer) => Test_IMaaImageListBuffer(type, buffer);
    private static void Test_IMaaImageListBuffer<T>(MaaTypes type, IMaaListBuffer<T> buffer) where T : IMaaImageBuffer
    {
        Assert.IsNotNull(buffer);
        var native = (buffer as MaaImageListBuffer)!;
        if (type == MaaTypes.Native)
        {
            Assert.IsNotNull(native);
            native = new MaaImageListBuffer(native.Handle);
        }

        Assert.IsFalse(
            buffer.IsInvalid);

        using var validValue = (T)GetNewImage(type).Buffer;
        Test_IMaaListBuffer_Empty(buffer);
        Test_IMaaListBuffer_NonReadonly(buffer, validValue);

        Assert.IsTrue(
            buffer.TryAdd(validValue));
        Test_IMaaListBuffer_NonEmpty(buffer);

        #region Func & Handle

        IList<byte[]>? dataList = default!;
        IList<Stream>? streamList = default!;
        if (type == MaaTypes.Native)
        {
            Assert.IsTrue(
                MaaImageListBuffer.TryGetEncodedDataList(native.Handle, out dataList));
            Assert.IsNotNull(
                dataList);
            Assert.AreEqual(
                buffer.Count, dataList.Count);

            Assert.IsTrue(
                MaaImageListBuffer.TrySetEncodedDataList(native.Handle, dataList));
            streamList = [.. dataList.Select(x => new MemoryStream(x))];
            Assert.IsTrue(
                MaaImageListBuffer.TrySetEncodedDataList(native.Handle, streamList));
            Assert.AreEqual(
                buffer.Count, dataList.Count * 3);

            Assert.IsTrue(
                MaaImageListBuffer.TryGetEncodedDataList(out var funcDataList, handle
                    => MaaImageListBuffer.TrySetEncodedDataList(handle, dataList)));
            Assert.AreEqual(
                funcDataList.Count, dataList.Count);

            Assert.IsTrue(
                MaaImageListBuffer.TrySetEncodedDataList(dataList, handle
                    => MaaImageListBuffer.TryGetEncodedDataList(handle, out funcDataList)));
            Assert.AreEqual(
                funcDataList.Count, dataList.Count);

            Assert.IsTrue(
                MaaImageListBuffer.TrySetEncodedDataList(streamList, handle
                    => MaaImageListBuffer.TryGetEncodedDataList(handle, out funcDataList)));
            Assert.AreEqual(
                funcDataList.Count, streamList.Count);
        }

        #endregion

        #region Invalid Data

        Assert.IsFalse(
            buffer.TryRemoveAt(ulong.MaxValue));

        if (type == MaaTypes.Native)
        {
            using var invalidImg = (T)(IMaaImageBuffer)new MaaImageBuffer();
            Assert.IsFalse(
                buffer.TryIndexOf(invalidImg, out var invalidIndex));
            Assert.AreEqual(
                ulong.MinValue, invalidIndex);

            Assert.IsTrue(
                MaaImageListBuffer.TrySetEncodedDataList(nint.Zero, Array.Empty<byte[]>()));
            Assert.IsTrue(
                MaaImageListBuffer.TrySetEncodedDataList(nint.Zero, Array.Empty<Stream>()));

            Assert.IsFalse(
                MaaImageListBuffer.TrySetEncodedDataList(nint.Zero, dataList));
            Assert.IsFalse(
                MaaImageListBuffer.TrySetEncodedDataList(nint.Zero, streamList));
            Assert.IsFalse(
                MaaImageListBuffer.TryGetEncodedDataList(nint.Zero, out dataList));
            Assert.IsNull(
                dataList);
            dataList = [];

            Assert.IsFalse(
                MaaImageListBuffer.TrySetEncodedDataList(Array.Empty<byte[]>(), static _ => false));
            Assert.IsFalse(
                MaaImageListBuffer.TrySetEncodedDataList(Array.Empty<Stream>(), static _ => false));
            Assert.IsFalse(
                MaaImageListBuffer.TryGetEncodedDataList(out dataList, static _ => false));
            Assert.IsNull(
                dataList);
        }

        #endregion

    }

    [TestMethod]
    [MaaData(MaaTypes.Native, nameof(Data))]
    public void Test_MaaStringListBuffer(MaaTypes type, MaaStringListBuffer buffer) => Test_IMaaStringListBuffer(type, buffer);
    private static void Test_IMaaStringListBuffer<T>(MaaTypes type, IMaaListBuffer<T> buffer) where T : IMaaStringBuffer
    {
        Assert.IsNotNull(buffer);
        var native = (buffer as MaaStringListBuffer)!;
        if (type == MaaTypes.Native)
        {
            Assert.IsNotNull(native);
            native = new MaaStringListBuffer(native.Handle);
        }

        Assert.IsFalse(
            buffer.IsInvalid);

        using var validValue = (T)NewStrings[type];
        Test_IMaaListBuffer_Empty(buffer);
        Test_IMaaListBuffer_NonReadonly(buffer, validValue);

        Assert.IsTrue(
            buffer.TryAdd(validValue));
        Test_IMaaListBuffer_NonEmpty(buffer);

        #region Func & Handle

        IList<string>? stringList = default!;
        if (type == MaaTypes.Native)
        {
            Assert.IsTrue(
                MaaStringListBuffer.TryGetList(native.Handle, out stringList));
            Assert.IsNotNull(
                stringList);
            Assert.AreEqual(
                buffer.Count, stringList.Count);

            Assert.IsTrue(
                MaaStringListBuffer.TrySetList(native.Handle, stringList));
            Assert.AreEqual(
                buffer.Count, stringList.Count * 2);

            Assert.IsTrue(
                MaaStringListBuffer.TryGetList(out var funcList, handle
                    => MaaStringListBuffer.TrySetList(handle, stringList)));
            Assert.AreEqual(
                funcList.Count, stringList.Count);

            Assert.IsTrue(
                MaaStringListBuffer.TrySetList(stringList, handle
                    => MaaStringListBuffer.TryGetList(handle, out funcList)));
            Assert.AreEqual(
                funcList.Count, stringList.Count);
        }

        #endregion

        #region Invalid Data

        Assert.IsFalse(
            buffer.TryRemoveAt(buffer.MaaSizeCount));

        if (type == MaaTypes.Native)
        {
            using var invalidStr = (T)(IMaaStringBuffer)new MaaStringBuffer();
            Assert.IsFalse(
                buffer.TryIndexOf(invalidStr, out var invalidIndex));
            Assert.AreEqual(
                ulong.MinValue, invalidIndex);

            Assert.IsTrue(
                MaaStringListBuffer.TrySetList(nint.Zero, []));

            Assert.IsFalse(
                MaaStringListBuffer.TrySetList(nint.Zero, stringList));
            Assert.IsFalse(
                MaaStringListBuffer.TryGetList(nint.Zero, out stringList));
            Assert.IsNull(
                stringList);
            stringList = [];

            Assert.IsFalse(
                MaaStringListBuffer.TrySetList([], static _ => false));
            Assert.IsFalse(
                MaaStringListBuffer.TryGetList(out stringList, static _ => false));
            Assert.IsNull(
                stringList);
        }

        #endregion

    }

    [TestMethod]
    [MaaData(MaaTypes.Native, nameof(Data))]
    public void Test_AdbDeviceListBuffer(MaaTypes type, AdbDeviceListBuffer buffer) => Test_IAdbDeviceListBuffer(type, buffer);
    private static void Test_IAdbDeviceListBuffer<T>(MaaTypes type, IMaaListBuffer<T> buffer) where T : AdbDeviceInfo
    {
        Assert.IsNotNull(buffer);
        var native = (buffer as AdbDeviceListBuffer)!;
        if (type == MaaTypes.Native)
        {
            Assert.IsNotNull(native);
            native = new AdbDeviceListBuffer(native.Handle);
        }

        Assert.IsFalse(
            buffer.IsInvalid);

        using var emptyBuffer = new AdbDeviceListBuffer();
        Test_IMaaListBuffer_Empty(emptyBuffer);

        Test_IMaaListBuffer_Readonly(buffer);
        Test_IMaaListBuffer_NonEmpty(buffer);

        #region Func & Handle

        IList<AdbDeviceInfo>? infoList = default!;
        if (type == MaaTypes.Native)
        {
            Assert.IsTrue(
                AdbDeviceListBuffer.TryGetList(native.Handle, out infoList));
            Assert.IsNotNull(
                infoList);
            Assert.AreEqual(
                buffer.Count, infoList.Count);

            Assert.IsTrue(
                AdbDeviceListBuffer.TryGetList(out var funcList,
                    Interop.Native.MaaToolkit.MaaToolkitAdbDeviceFind));
            Assert.AreEqual(
                funcList.Count, infoList.Count);
        }

        #endregion

        #region Invalid Data

        if (type == MaaTypes.Native)
        {
            var invalidInfo = (T)(AdbDeviceInfo)new AdbDeviceListBuffer.MaaToolkitAdbDeviceInfo(nint.Zero);
            Assert.IsFalse(
                buffer.TryIndexOf(invalidInfo, out var invalidIndex));
            Assert.AreEqual(
                ulong.MinValue, invalidIndex);

            Assert.IsFalse(
                AdbDeviceListBuffer.TryGetList(nint.Zero, out infoList));
            Assert.IsNull(
                infoList);
            infoList = []; Assert.IsNotNull(infoList);

            Assert.IsFalse(
                AdbDeviceListBuffer.TryGetList(out infoList, static _ => false));
            Assert.IsNull(
                infoList);
        }

        #endregion

    }

#if MAA_WIN32
    [TestMethod]
    [MaaData(MaaTypes.Native, nameof(Data))]
    public void Test_DesktopWindowListBuffer(MaaTypes type, DesktopWindowListBuffer buffer) => Test_IDesktopWindowListBuffer(type, buffer);
    private static void Test_IDesktopWindowListBuffer<T>(MaaTypes type, IMaaListBuffer<T> buffer) where T : DesktopWindowInfo
    {
        Assert.IsNotNull(buffer);
        var native = (buffer as DesktopWindowListBuffer)!;
        if (type == MaaTypes.Native)
        {
            Assert.IsNotNull(native);
            native = new DesktopWindowListBuffer(native.Handle);
        }

        Assert.IsFalse(
            buffer.IsInvalid);

        using var emptyBuffer = new DesktopWindowListBuffer();
        Test_IMaaListBuffer_Empty(emptyBuffer);

        Test_IMaaListBuffer_Readonly(buffer);
        Test_IMaaListBuffer_NonEmpty(buffer);

        #region Func & Handle

        IList<DesktopWindowInfo>? funcList = default!;
        IList<DesktopWindowInfo>? infoList = default!;
        if (type == MaaTypes.Native)
        {
            Assert.IsTrue(
                DesktopWindowListBuffer.TryGetList(native.Handle, out infoList));
            Assert.IsNotNull(
                infoList);
            Assert.AreEqual(
                buffer.Count, infoList.Count);

            Assert.IsTrue(
                DesktopWindowListBuffer.TryGetList(out funcList,
                    Interop.Native.MaaToolkit.MaaToolkitDesktopWindowFindAll));
            // Assert.AreEqual(
            //     funcList.Count, infoList.Count);
        }

        #endregion

        #region Invalid Data

        if (type == MaaTypes.Native)
        {
            var invalidInfo = (T)funcList[0];
            Assert.IsFalse(
                buffer.TryIndexOf(invalidInfo, out var invalidIndex));
            Assert.AreEqual(
                ulong.MinValue, invalidIndex);

            Assert.IsFalse(
                DesktopWindowListBuffer.TryGetList(nint.Zero, out infoList));
            Assert.IsNull(
                infoList);
            infoList = []; Assert.IsNotNull(infoList);

            Assert.IsFalse(
                DesktopWindowListBuffer.TryGetList(out infoList, static _ => false));
            Assert.IsNull(
                infoList);
        }

        #endregion

    }
#endif

    private static void Test_IMaaListBuffer_Readonly<T>(IMaaListBuffer<T> buffer)
    {
        _ = Assert.ThrowsException<NotSupportedException>(() =>
            buffer.TryCopyTo(buffer));
        _ = Assert.ThrowsException<NotSupportedException>(() =>
            buffer.TryAdd(default!));
        _ = Assert.ThrowsException<NotSupportedException>(() =>
            buffer.TryRemoveAt(0));
        _ = Assert.ThrowsException<NotSupportedException>(() =>
            buffer.TryClear());

        #region IList<T>

        _ = Assert.ThrowsException<NotSupportedException>(() =>
            ((IList<T>)buffer)[0] = default!);
        _ = Assert.ThrowsException<NotSupportedException>(() =>
            buffer.Insert(0, default!));
        _ = Assert.ThrowsException<NotSupportedException>(() =>
            buffer.RemoveAt(0));

        Assert.IsTrue(
            buffer.IsReadOnly);
        _ = Assert.ThrowsException<NotSupportedException>(() =>
            buffer.Add(default!));
        _ = Assert.ThrowsException<NotSupportedException>(
            buffer.Clear);
        _ = Assert.ThrowsException<NotSupportedException>(() =>
            buffer.Remove(default!));

        var array = new T[buffer.Count];
        buffer.CopyTo(array, 0);

        #endregion
    }

    private static void Test_IMaaListBuffer_NonReadonly<T>(IMaaListBuffer<T> buffer, T validValue)
    {
        Assert.IsFalse(
            buffer.TryAdd(default!));
        Assert.IsFalse(
            buffer.TryCopyTo(default!));

        var index = buffer.MaaSizeCount;
        Assert.IsTrue(
            buffer.TryAdd(validValue));
        Assert.IsTrue( // have 2 valid value
            buffer.TryCopyTo(buffer));

        Assert.IsTrue(
            buffer.TryIndexOf(validValue, out var validValueIndex));
        Assert.AreEqual( // the first validValue
            index, validValueIndex);
        Assert.IsTrue(
            buffer.TryRemoveAt(index));

        Assert.IsTrue(
            buffer.TryIndexOf(validValue, out validValueIndex));
        Assert.AreEqual( // the second validValue
            index, validValueIndex);
        Assert.IsTrue(
            buffer.TryClear());


        #region IList<T>

        _ = Assert.ThrowsException<NotSupportedException>(() =>
            ((IList<T>)buffer)[0] = validValue);
        _ = Assert.ThrowsException<NotSupportedException>(() =>
            buffer.Insert(0, validValue));

        Assert.IsFalse(
            buffer.IsReadOnly);
        buffer.Add(validValue);
        buffer.Add(validValue);
        var array = new T[buffer.Count];
        buffer.CopyTo(array, 0);
        CollectionAssert.AllItemsAreNotNull(array);

        buffer.RemoveAt(0);
        Assert.IsTrue(
            buffer.Remove(validValue));
        buffer.Clear();

        #endregion
    }

    private static void Test_IMaaListBuffer_Empty<T>(IMaaListBuffer<T> buffer)
    {
        Assert.IsTrue(
            buffer.IsEmpty);
        Assert.AreEqual(
            ulong.MinValue, buffer.MaaSizeCount);

        _ = Assert.ThrowsException<MaaInteroperationException>(() =>
            buffer[0]);
        Assert.IsFalse(
            buffer.TryIndexOf(default!, out _));

        #region IList<T>

        _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            ((IList<T>)buffer)[0]);
        Assert.AreEqual(
            -1, buffer.IndexOf(default!));

        Assert.AreEqual(
            0, buffer.Count);
        Assert.IsFalse(
            buffer.Contains(default!));

        Assert.IsNotNull( // IEnumerable<T>
            buffer.ToList());
        foreach (var item in buffer) // IEnumerable
        {
            Assert.IsNotNull(item);
        }

        #endregion
    }

    private static void Test_IMaaListBuffer_NonEmpty<T>(IMaaListBuffer<T> buffer)
    {
        Assert.IsFalse(
            buffer.IsEmpty);
        Assert.AreNotEqual(
            ulong.MinValue, buffer.MaaSizeCount);

        var t = buffer[0];
        Assert.IsNotNull(t);
        Assert.IsTrue(
            buffer.TryIndexOf(t, out _));

        #region IList<T>

        Assert.IsNotNull(
            ((IList<T>)buffer)[0]);
        Assert.AreEqual(
            0, buffer.IndexOf(t));

        Assert.AreNotEqual(
            0, buffer.Count);
        Assert.IsTrue(
            buffer.Contains(t));

        Assert.IsNotNull( // IEnumerable<T>
            buffer.ToList());
        foreach (var item in buffer) // IEnumerable
        {
            Assert.IsNotNull(item);
        }

        #endregion
    }
}

internal sealed class TestImageBuffer : IMaaImageBuffer
{
    public bool TryCopyTo(IMaaImageBuffer buffer) => throw new NotImplementedException();
    public void Dispose() => throw new NotImplementedException();
    public bool IsInvalid => throw new NotImplementedException();
    public bool ThrowOnInvalid
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }
    public bool IsStateless => throw new NotImplementedException();
    public bool IsEmpty => throw new NotImplementedException();
    public bool TryClear() => throw new NotImplementedException();
    public ImageInfo GetInfo() => throw new NotImplementedException();
    public int Width => throw new NotImplementedException();
    public int Height => throw new NotImplementedException();
    public int Channels => throw new NotImplementedException();
    public int Type => throw new NotImplementedException();
    public bool TryGetEncodedData([MaybeNullWhen(false)] out byte[] data) => throw new NotImplementedException();
    public bool TryGetEncodedData([MaybeNullWhen(false)] out Stream data) => throw new NotImplementedException();
    public bool TryGetEncodedData(out ReadOnlySpan<byte> data) => throw new NotImplementedException();
    public bool TrySetEncodedData(byte[] data) => throw new NotImplementedException();
    public bool TrySetEncodedData(Stream data) => throw new NotImplementedException();
    public bool TrySetEncodedData(ReadOnlySpan<byte> data) => throw new NotImplementedException();
}

internal sealed class TestStringBuffer : IMaaStringBuffer
{
    public bool TryCopyTo(IMaaStringBuffer buffer) => throw new NotImplementedException();
    public void Dispose() => throw new NotImplementedException();
    public bool IsInvalid => throw new NotImplementedException();
    public bool ThrowOnInvalid
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }
    public bool IsStateless => throw new NotImplementedException();
    public bool IsEmpty => throw new NotImplementedException();
    public ulong Size => throw new NotImplementedException();
    public bool TryClear() => throw new NotImplementedException();
    public bool TryGetValue([MaybeNullWhen(false)] out string str) => throw new NotImplementedException();
    public bool TrySetValue(string str, bool useEx = true) => throw new NotImplementedException();
}

internal sealed class TestRectBuffer : IMaaRectBuffer
{
    public bool TryCopyTo(IMaaRectBuffer buffer) => throw new NotImplementedException();
    public void Dispose() => throw new NotImplementedException();
    public bool IsInvalid => throw new NotImplementedException();
    public bool ThrowOnInvalid
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }
    public bool IsStateless => throw new NotImplementedException();
    public int X => throw new NotImplementedException();
    public int Y => throw new NotImplementedException();
    public int Width => throw new NotImplementedException();
    public int Height => throw new NotImplementedException();
    public bool TrySetValues(int x, int y, int width, int height) => throw new NotImplementedException();
    public bool TryGetValues(out int x, out int y, out int width, out int height) => throw new NotImplementedException();
    public void Deconstruct(out int x, out int y, out int width, out int height) => throw new NotImplementedException();
}
