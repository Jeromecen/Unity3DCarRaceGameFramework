һ��windows�¹���Excel��
1����װ����:
1).�Ȱ���python2.7.7��cmd��ִ��python�ɹ����ɡ�
2).��װ���������protobuf-2.5.0��xlrd-1.0.0.tar����װ�����ǽ�ѹ�ļ���������ѹ�ļ���setup.py��(protobuf-2.5.0��python��Ŀ¼��)ִ��:
python setup.py install

2��ִ��ת��:
1������
����Ŀ��c1\Excels����������ĸ�ʽ�ο���goods_info_template.xls����ϸ��ʽ��Excels�µģ�xls_deploy_tool.py�ڵ�ע�͡�
Sheet��Ҫ���д��������úͱ���һ���������Ա���Ϊǰ׺��
����Ϊ.xls��ʽ(97-2004�汾)��
2��ת��
cmd ��c1\tools\ExcelToProtoĿ¼�£�ִ�����
build Scenes SCENE_INFO
PS:����Scenes�Ǳ���������SCENE_INFO��Sheet����(��д)��

����MacOS�¹���Excel��
1����װ����
1)��װExcels
http://bbs.feng.com/read-htm-tid-9704285.html
2).��װprotobuffer
http://www.cnblogs.com/yanghuahui/p/4448728.html
3).��װmono
�ο���http://blog.csdn.net/zhuangyou123/article/details/51482029
mono��ַ��http://www.mono-project.com/download/
2��ִ��ת��
1������
��windows��һ����
2��ת��
cd��c1/tools/ExcelToProto/mactoolĿ¼�£�ִ�����
����һ��ִ�У���ȡִ��Ȩ�ޣ�chmod x+u build.sh
ת��
./build Scenes SCENE_INFO
PS:����Scenes�Ǳ���������SCENE_INFO��Sheet����(��д)��
