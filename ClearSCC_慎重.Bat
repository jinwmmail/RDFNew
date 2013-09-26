@echo off 
echo 正在清除临时文件，请稍等......

cmd /c del /q /s "*.*scc"

attrib /s /d -h -s *.*

attrib /s /d -r *.*

echo 操作完成！ 
