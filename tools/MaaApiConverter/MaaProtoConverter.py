import os

cur_dir = os.path.dirname(__file__)
protos_path = os.path.abspath(os.path.join(cur_dir, '../../src/MaaFramework.Binding.Grpc/Protos/'))

for file in os.listdir(protos_path):
    if not file.endswith('.proto'):
        continue
    with open(os.path.join(protos_path, file), 'r+') as f:
        lines = f.readlines()
        if 'option csharp_namespace' in ''.join(lines):
            continue
        for i, line in enumerate(lines):
            if line.strip() == 'package maarpc;':
                lines.insert(i + 1, '\noption csharp_namespace = "MaaFramework.Binding.Grpc.Interop";\n')
                break
        f.seek(0)
        f.writelines(lines)
