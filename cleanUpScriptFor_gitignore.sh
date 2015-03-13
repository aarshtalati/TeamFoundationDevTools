
# $ sh cleanUpScriptFor_gitignore.sh

find . -name "*.suo" -type f | xargs rm -f

cd TeamFoundationDevTools/

find . -name "*.user" -type f | xargs rm -f
find . -name "*.userosscache" -type f | xargs rm -f
find . -name "*.sln.docstates" -type f | xargs rm -f
find . -name "*.userprefs" -type f | xargs rm -f
find . -name "*.VisualState.xml" -type f | xargs rm -f
find . -name "*.JustCode" -type f | xargs rm -f
find . -name "*.nupkg" -type f | xargs rm -f
find . -name "buildlog.*" -type f | xargs rm -f

find . -name "UpgradeLog*.XML" -type f | xargs rm -f
find . -name "UpgradeLog*.htm" -type f | xargs rm -f

rm -rf debug/
rm -rf debugpublic/
rm -rf release/
rm -rf releases/
rm -rf x64/
rm -rf x86/
rm -rf build/
rm -rf bld/
rm -rf bin/
rm -rf obj/
rm -rf vs/
rm -rf ClientBin/
rm -rf testresult*/

cd ..