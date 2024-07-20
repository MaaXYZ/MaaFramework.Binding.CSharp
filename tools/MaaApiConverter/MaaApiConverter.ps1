rm -r temp
mkdir temp
cd temp
mkdir .config

# Template
curl https://raw.githubusercontent.com/MaaXYZ/MaaApiConverter/v1/gen/Templates/CSharpTemplate.cs -o CSharpTemplate.cs
# Index & Model
curl https://maaxyz.github.io/MaaFramework/index.json -o index.json
curl https://raw.githubusercontent.com/MaaXYZ/MaaApiConverter/v1/src/Models/MaaApiDocument.cs -o MaaApiDocument.cs # -r:System.Data.Common.dll
# Tools
curl https://raw.githubusercontent.com/MaaXYZ/MaaApiConverter/v1/gen/Tools/.config/dotnet-tools.json -o .config/dotnet-tools.json
curl https://raw.githubusercontent.com/MaaXYZ/MaaApiConverter/v1/gen/Tools/GenExtension.cs -o GenExtension.cs
curl https://raw.githubusercontent.com/MaaXYZ/MaaApiConverter/v1/gen/Tools/NamingConverter.cs -o NamingConverter.cs # -r:System.Text.Json.dll

cat CSharpTemplate.cs   >  MaaApiConvert.cs
cat MaaApiDocument.cs   >> MaaApiConvert.cs
cat GenExtension.cs     >> MaaApiConvert.cs
cat NamingConverter.cs  >> MaaApiConvert.cs

dotnet tool restore
dotnet dotnet-codegencs template run MaaApiConvert.cs index.json -r:System.Data.Common.dll -r:System.Text.Json.dll

cd ..
ls -Directory ../../src/MaaFramework.Binding.Native/Interop/ | rm -r
cp -r -Force temp/src ../../
rm -r temp
echo
