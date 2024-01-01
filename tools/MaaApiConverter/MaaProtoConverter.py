import os

cur_dir = os.path.dirname(__file__)
protos_path = os.path.abspath(os.path.join(cur_dir, '../../src/MaaFramework.Binding.Grpc/Protos/'))
option_code = 'option csharp_namespace = "MaaFramework.Binding.Interop.Grpc";'

for file in os.listdir(protos_path):
    if not file.endswith('.proto'):
        continue
    with open(os.path.join(protos_path, file), 'r+') as f:
        lines = f.readlines()
        index = 0
        code = 'option csharp_namespace = "MaaFramework.Binding.Interop.Grpc";'
        for i, line in enumerate(lines):
            if 'option csharp_namespace' in line:
                index = i
                code = f'{option_code}\n'
                lines.pop(i)
                break
            elif 'package maarpc' in line:
                index = i + 1
                code = f'\n{option_code}\n'
        lines.insert(index, code)
        f.seek(0)
        f.writelines(lines)
