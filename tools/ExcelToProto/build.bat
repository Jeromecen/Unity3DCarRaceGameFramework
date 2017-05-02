@echo off
set XLS_NAME=%1
set SHEET_NAME=%2

echo.
echo =========Compilation of %XLS_NAME%.xls=========

::---------------------------------------------------
::第一步，将xls经过xls_deploy_tool转成data和proto
::---------------------------------------------------
set STEP1_XLS2PROTO_PATH=.\step1_xls2proto
@echo on
cd tool\%STEP1_XLS2PROTO_PATH%

@echo off
echo TRY TO DELETE TEMP FILES:
del *_pb2.py
del *_pb2.pyc
del *.proto
del *.data
del *.log
del *.txt

@echo on
python xls_deploy_tool.py %SHEET_NAME% ..\..\..\..\Excels\%XLS_NAME%.xls

::---------------------------------------------------
::第二步：把proto翻译成cs
::---------------------------------------------------
cd ..

set STEP2_PROTO2CS_PATH=.\step2_proto2cs
set PROTO_DESC=proto.protobin

cd %STEP2_PROTO2CS_PATH%

@echo off
echo TRY TO DELETE TEMP FILES:
del *.cs
del *.protobin
del *.txt


@echo on
dir ..\%STEP1_XLS2PROTO_PATH%\*.proto /b  > protolist.txt

@echo on
for /f "delims=." %%i in (protolist.txt) do protoc ..\%STEP1_XLS2PROTO_PATH%\*.proto --descriptor_set_out=%PROTO_DESC% --proto_path=..\%STEP1_XLS2PROTO_PATH% 
for /f "delims=." %%i in (protolist.txt) do ProtoGen\protogen -i:%PROTO_DESC% -o:%%i.cs


cd ..

::---------------------------------------------------
::第三步：将data和cs拷到Assets里
::---------------------------------------------------

@echo off
set OUT_PATH=..\..\..\client\Assets
set DATA_DEST=DataConfig
set CS_DEST=Scripts\Server\DataConfig\ProtoGen


@echo on
copy %STEP1_XLS2PROTO_PATH%\*.data %OUT_PATH%\%DATA_DEST%
copy %STEP2_PROTO2CS_PATH%\*.cs %OUT_PATH%\%CS_DEST%

::---------------------------------------------------
::第四步：清除中间文件
::---------------------------------------------------
@echo off
echo TRY TO DELETE TEMP FILES:
cd %STEP1_XLS2PROTO_PATH%
del *_pb2.py
del *_pb2.pyc
del *.proto
del *.data
del *.log
del *.txt
cd ..
cd %STEP2_PROTO2CS_PATH%
del *.cs
del *.protobin
del *.txt
cd ..

::---------------------------------------------------
::第五步：结束
::---------------------------------------------------
cd ..
pause
@echo on