import os
import re
import requests
import pyperclip
'''
include/MaaFramework/Utility/MaaUtility.h Press any key to continue.
include/MaaFramework/Utility/MaaUtility.h Copied to clipboard.
include/MaaRpc/MaaFramework/MaaDef.h Press any key to continue.
include/MaaRpc/MaaFramework/MaaDef.h Copied to clipboard.
include/MaaToolKit/MaaToolKitDef.h Press any key to continue.
'''
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

    # 匹配函数声明
    pattern = pattern = re.compile(r'(\w+)\s+(?:MAA_.*?_API)\s+(\w+)\s*\(([\s\S]*?)\)\s*;', re.DOTALL)
    matches = pattern.finditer(content)

    # 转换为 C# 代码
    for match in matches:
        return_type = match.group(1)
        function_name = match.group(2)
        parameters = ' '.join(match.group(3).split())

        # 转换参数类型
        parameters = parameters.replace('int32_t*', 'ref int32_t')
        parameters = parameters.replace('char*', 'nint')
        parameters = parameters.replace('void*', 'nint')
        parameters = re.sub(r'\bMaaOptionValue\b', 'ref MaaOptionValue', parameters)
        parameters = parameters.replace('MaaCustomActionHandle', 'ref MaaCustomActionApi')
        parameters = parameters.replace('MaaCustomControllerHandle', 'ref MaaCustomControllerApi')
        parameters = parameters.replace('MaaCustomRecognizerHandle', 'ref MaaCustomRecognizerApi')
        parameters = parameters.replace('MaaRectHandle', 'ref MaaRectApi')
        parameters = parameters.replace('MaaStringView', '[MarshalAs(UnmanagedType.LPUTF8Str)] string')

        csharp_code = f'    [LibraryImport("{dll_name}")]\n    public static partial {return_type} {function_name}({parameters});\n'
        csharp_codes.append(csharp_code)

    csharp_codes.append(f'#endregion\n')
    pyperclip.copy('\n'.join(csharp_codes))
    print(f'{header_path} Copied to clipboard.')
    input(f'Press any key to continue.')

def get_version() -> str:
    url = f'https://api.github.com/repos/{repo}/releases/latest'
    json = requests.get(url).json()
    try:
        print(json['message']) 
        exit(1)
        # return 'main'
    except:
        return json['tag_name']

def get_header(header_path: str):
    url = f'https://raw.githubusercontent.com/{repo}/{version}/{header_path}'
    return requests.get(url).text

def get_includes(header_path: str) -> list[str]:
    lines = get_header(header_path).split('\n')
    path = os.path.split(header_path)[0] + '/'
    return [ path + line.split(' ')[1].strip('"') for line in lines if line.startswith('#include')]


version = get_version()
for path in api_path:
    convert_cpp_header_to_csharp(path)
    includes = get_includes(path)
    for include in includes:
        convert_cpp_header_to_csharp(include)
