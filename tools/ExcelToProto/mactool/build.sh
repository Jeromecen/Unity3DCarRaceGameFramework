#!/bin/sh  
#.xls to .data and .proto
XLS_NAME=$1
SHEET_NAME=$2
XLS_SOURCE_PATH=../../../../../Excels/${XLS_NAME}.xls
#.proto to cs
cd ./protogen-master
STEP1_XLS2PROTO_PATH=step1_xls2proto
STEP2_PROTO2CS_PATH=step2_proto2cs
#1 .excel to .data and .proto
# delete cur file and copy to dir
cd ${STEP1_XLS2PROTO_PATH}
rm *.txt
rm *.proto
rm *.data
rm *.log
rm *_pb2.py
rm *_pb2.pyc
echo ${SHEET_NAME}
echo ${XLS_SOURCE_PATH}
python xls_deploy_tool.py ${SHEET_NAME} ${XLS_SOURCE_PATH}
#2 .proto to .protobin and .protobin to .cs
cd ../${STEP2_PROTO2CS_PATH}
rm *.protobin
rm *.proto
rm *.txt
rm *.cs
PRODESC=proto.protobin
ls ../${STEP1_XLS2PROTO_PATH}/*.proto > protolist.txt
for filePath in $(cat protolist.txt) 
do
	fileName=$(basename $filePath .proto)
    protoc ../${STEP1_XLS2PROTO_PATH}/*.proto --descriptor_set_out=${fileName}.protobin --proto_path=../${STEP1_XLS2PROTO_PATH} 
	mono protogen.exe -i:${fileName}.protobin -o:${fileName}.cs
	echo ${fileName}
done
# echo ../${STEP1_XLS2PROTO_PATH}/*.proto echo $(basename $filePath .protobin)
#protoc ../${STEP1_XLS2PROTO_PATH}/*.proto --descriptor_set_out=${PRODESC}
#for filePath in 'cat $protolist.txt' do mono protogen.exe -i:${PRODESC} -o:$(basename $filePath .protobin).cs
#3. copy to work space
OUT_PATH=../../../../client/Assets
DATA_DEST=${OUT_PATH}/DataConfig
CS_DEST=${OUT_PATH}/Scripts/Server/DataConfig/ProtoGen
cd ../
cp ${STEP1_XLS2PROTO_PATH}/*.data ${DATA_DEST}
cp ${STEP2_PROTO2CS_PATH}/*.cs ${CS_DEST}
echo 'successful!'