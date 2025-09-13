$tag = git describe --tags --match v*
$latest = gh release view --json tagName --jq .tagName
python changelog_generator.py --tag "$tag" --latest "$latest"
