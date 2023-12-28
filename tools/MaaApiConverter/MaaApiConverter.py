import os
import re
import sys
import requests
import pyperclip

repo = 'MaaAssistantArknights/MaaFramework'
api_path = [
    'include/MaaFramework/MaaAPI.h',
    'include/MaaRpc/MaaRpc.h',
    'include/MaaToolKit/MaaToolKitAPI.h'
]

def convert_cpp_header_to_csharp(header_path: str):
    dll_name = header_path.split('/')[1]
    csharp_codes = []
    csharp_codes.append(f'\n#region {header_path}, version: {version}.\n')

    # 获取文件
    content = get_header(header_path)
    # 输出过时信息
    print('\n'.join(line for line in content.split('\n') if 'deprecate' in line.lower()))

    # 匹配函数指针声明
    pattern = re.compile(r'(\w+)\s+\(\*(\w+)\)\s*\(([\s\S]*?)\)\s*;', re.DOTALL)
    matches = pattern.finditer(content)
    # 转换为 C# 代码
    for match in matches:
        return_type = match.group(1)
        function_name = ''.join(word[0].upper() + word[1:] for word in match.group(2).split('_'))
        parameters = ' '.join(match.group(3).replace('//\n', '').split())
        # 转换名称和参数类型
        function_name = 'Abort' if function_name == 'Stop' else function_name
        parameters = parameters.replace('int32_t*', 'ref int32_t')
        parameters = parameters.replace('const MaaImageBufferHandle', 'MaaImageBufferHandle')
        parameters = ','.join(p for p in parameters.split(',') if 'TransparentArg' not in p)
        csharp_code = f'    public delegate {return_type} {function_name}({parameters});\n'
        csharp_codes.append(csharp_code)

    # 匹配函数声明
    pattern = re.compile(r'(\w+)\s+(?:MAA_.*?_API)\s+(\w+)\s*\(([\s\S]*?)\)\s*;', re.DOTALL)
    matches = pattern.finditer(content)
    
    # 转换为 C# 代码
    for match in matches:
        return_type = match.group(1)
        function_name = match.group(2)
        parameters = ' '.join(match.group(3).replace('//\n', '').split())

        # 特定 API
        special_api_dict = {
            'MaaSetStringEx': {'MaaStringView': 'ref byte'},
        }
        if function_name in special_api_dict.keys():
            ps = special_api_dict['MaaSetStringEx']
            while len(ps):
                p1, p2 = ps.popitem()
                parameters = parameters.replace(p1, p2)
        else:
            # 转换参数类型
            parameters = parameters.replace('int32_t*', 'ref int32_t')
            parameters = parameters.replace('char*', 'nint')
            parameters = parameters.replace('void*', 'nint')
            parameters = re.sub(r'\bMaaOptionValue\b', 'ref MaaOptionValue', parameters) # MaaOptionValue 而不是 MaaOptionValueSize
            parameters = parameters.replace('MaaCustomActionHandle', 'ref MaaCustomActionApi')
            parameters = parameters.replace('MaaCustomControllerHandle', 'ref MaaCustomControllerApi')
            parameters = parameters.replace('MaaCustomRecognizerHandle', 'ref MaaCustomRecognizerApi')
            # parameters = parameters.replace('MaaRectHandle', 'ref MaaRect')
            parameters = parameters.replace('MaaStringView', '[MarshalAs(UnmanagedType.LPUTF8Str)] string')

        # 过时 API
        obsolete_api_dict = {
            'MaaAdbControllerCreate': 'MaaAdbControllerCreateV2',
        }
        if function_name in obsolete_api_dict.keys():
            obsolete_explanation = f'    [Obsolete("This API {function_name} is about to be deprecated. Please use {obsolete_api_dict[function_name]} instead.")]'
            csharp_codes.append(obsolete_explanation)

        csharp_code = f'    [LibraryImport("{dll_name}")]\n    public static partial {return_type} {function_name}({parameters});\n'
        csharp_codes.append(csharp_code)

    csharp_codes.append(f'#endregion\n')
    pyperclip.copy('\n'.join(csharp_codes))
    print(f'{header_path} Copied to clipboard.')
    input(f'Press any key to continue.')

def get_version() -> str:
    if len(sys.argv) > 1:
        return sys.argv[1]
    url = f'https://api.github.com/repos/{repo}/releases/latest'
    json = requests.get(url).json()
    if 'message' in json:
        print(json['message']) 
        exit(1)
    else:
        return json['tag_name']

def get_header(header_path: str):
    url = f'https://raw.githubusercontent.com/{repo}/{version}/{header_path}'
    return requests.get(url).text

def get_includes(header_path: str) -> list[str]:
    lines = get_header(header_path).split('\n')
    path = os.path.split(header_path)[0] + '/'
    return [ path + line.split(' ')[1].strip('"') for line in lines if line.startswith('#include')]


version = get_version()
print("Current MaaFramework Version:", version)
for path in api_path:
    convert_cpp_header_to_csharp(path)
    includes = get_includes(path)
    for include in includes:
        convert_cpp_header_to_csharp(include)
