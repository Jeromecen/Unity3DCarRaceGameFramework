一、windows下构建Excel表：
1、安装环境:
1).先按照python2.7.7，cmd下执行python成功即可。
2).安装第三方插件protobuf-2.5.0和xlrd-1.0.0.tar，安装方法是解压文件，均到解压文件的setup.py下(protobuf-2.5.0在python子目录下)执行:
python setup.py install

2、执行转表:
1）建表
在项目的c1\Excels建表，建立表的格式参考：goods_info_template.xls，详细格式见Excels下的：xls_deploy_tool.py内的注释。
Sheet名要求大写，名字最好和表名一样，或者以表名为前缀。
保存为.xls格式(97-2004版本)。
2）转表
cmd 到c1\tools\ExcelToProto目录下，执行命令：
build Scenes SCENE_INFO
PS:参数Scenes是表明，参数SCENE_INFO是Sheet名字(大写)。

二、MacOS下构建Excel表：
1、安装环境
1)安装Excels
http://bbs.feng.com/read-htm-tid-9704285.html
2).安装protobuffer
http://www.cnblogs.com/yanghuahui/p/4448728.html
3).安装mono
参考：http://blog.csdn.net/zhuangyou123/article/details/51482029
mono地址：http://www.mono-project.com/download/
2、执行转表
1）建表
和windows下一样。
2）转表
cd到c1/tools/ExcelToProto/mactool目录下，执行命令：
若第一次执行，获取执行权限：chmod x+u build.sh
转表：
./build Scenes SCENE_INFO
PS:参数Scenes是表明，参数SCENE_INFO是Sheet名字(大写)。
