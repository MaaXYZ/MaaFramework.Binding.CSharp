import re
import requests
import pyperclip

def convert_cpp_header_to_csharp(header_path: str):
    csharp_codes = []

    # 获取文件的最新 Commit 值
    repo_url = f'https://api.github.com/repos/MaaAssistantArknights/MaaFramework/commits?path={header_path}'
    response = requests.get(repo_url)
    commit_hash = response.json()[0]['sha']
    commit_title = response.json()[0]['commit']['message']  # 获取 commit 的标题
    csharp_codes.append(f'\n#region {header_path}, commit title: {commit_title}, commit hash: {commit_hash}.\n')

    # 获取文件
    url = f'https://raw.githubusercontent.com/MaaAssistantArknights/MaaFramework/main/{header_path}'
    response = requests.get(url)
    content = response.text

    # 匹配函数声明
    pattern = re.compile(r'(\w+)\s+(?:MAA_FRAMEWORK_API|MAA_TOOLKIT_API)\s+(\w+)\s*\(([\s\S]*?)\)\s*;', re.DOTALL)
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

        csharp_code = f'    [LibraryImport("MaaFramework")]\n    public static partial {return_type} {function_name}({parameters});\n'
        csharp_codes.append(csharp_code)

    csharp_codes.append(f'#endregion\n')
    pyperclip.copy('\n'.join(csharp_codes))
    print('Copied to clipboard.')

convert_cpp_header_to_csharp('include/MaaFramework/MaaAPI.h')
