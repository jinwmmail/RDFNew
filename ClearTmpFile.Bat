@echo off 
echo 正在清除临时文件，请稍等......

ATTRIB -r -a -s -h RDFNew.Web\Bin /s /d
rd /s /q RDFNew.Web\Bin
ATTRIB -r -a -s -h RDFNew.Web\obj /s /d
rd /s /q RDFNew.Web\obj

ATTRIB -r -a -s -h RDFNew.Module\Bin /s /d
rd /s /q RDFNew.Module\Bin
ATTRIB -r -a -s -h RDFNew.Module\obj /s /d
rd /s /q RDFNew.Module\obj

echo 清除完成！ 