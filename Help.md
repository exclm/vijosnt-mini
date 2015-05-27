# 使用帮助 #
## 目录 ##

## 关于Vijos-Mini ##
VijosNT-Mini是VijosNT的一个实验性分支, 主要目标在于提高性能以及降低使用难度, 在 [VijosNT](http://code.google.com/p/vijosnt) 的基础上重写了所有的代码, 特别针对配置较差（CPU 数量小于 8，内存小于 16GB）的计算机进行了优化(大量使用完成函数代替线程等待), 并实现了一个友好的用户界面。

![http://vijosnt-mini.googlecode.com/svn/wiki/Img1.png](http://vijosnt-mini.googlecode.com/svn/wiki/Img1.png)

初次使用VijosNT-Mini前，请先仔细阅读使用帮助, 以免发生意外。

## 获得VijosNT-Mini ##
如您需要使用VijosNT-Mini，请先下载一个合适您计算机的版本或下载源代码自行编译。

使用前必须安装 [.Net Framework 2.0](http://www.microsoft.com/downloads/details.aspx?displaylang=zh-cn&FamilyID=0856eacb-4362-4b0d-8edd-aab15c5e04f5) 或[更高版本](http://www.microsoft.com/downloads/details.aspx?displaylang=zh-cn&FamilyID=9cfb2d51-5ff4-4491-b0e5-b386f32c0992)。

Vijos-Mini程序下载地址:http://code.google.com/p/vijosnt-mini/downloads/list

Vijos-Mini源代码下载地址: http://vijosnt-mini.googlecode.com/svn/trunk/(您可以使用TortoiseSVN等SVN软件自行下载源代码并使用Visual Studio 2010及以上版本编译，编译前，请先安装[SQLite.Net](http://sqlite.phxsoftware.com/))

下载Vijos-Mini后，您将解压缩得到两个版本的文件，分别是x86和x64，请根据您的操作系统选择合适的版本，32位系统请使用x86版本，64位系统请使用x64版本。如您自行下载源代码编译，请运行publish.bat。

## 文件说明 ##
**第一次使用需要两个文件**

System.Data.SQLite.dll

VijosNT Mini.exe

其中

System.Data.SQLite.dll是SQLite运行必备的

VijosNT Mini.exe是VijosNT-Mini的主程序


## 首次使用 ##
打开VijosNT Mini.exe

双击悬浮窗或通知区域图标打开控制台界面，也可以右键悬浮窗或控制台点击“启动控制台”
点击右侧的“启动”启动VijosNT Mini服务，期间杀毒软件可能提示有程序正在加载服务或驱动，请点击允许。此时悬浮窗以及通知区域图标变为绿色，表示服务安装成功。

**初次使用之后，可能会产生以下两个文件(夹)**

VijosNT.db3

Temp目录

其中VijosNT.db3是本地数据库文件，用于存储本地测数据信息以及配置
Temp目录中存放临时文件，一般用完即删，不会积累很多文件

## 配置编译器 ##
点击控制台左边的“编译器映射”，点击右侧的“添加旁边的小箭头”添加编译器
### 目前支持直接添加的编译器的语言 ###
**C**

C++

Free Pascal

Visual C

Visual C++

C#

Visual Basic.Net

JAVA

Python

### 添加编译器 ###
此处以C,C++和Free Pascal为例添加编译器。

#### 添加C的编译器 ####
点击右侧“添加旁边的小箭头”选择MinGW菜单下的C。若VijosNT-Mini能找到编译器路径，将弹出窗口询问编译器路径是否正确。如该路径正确，点击是；反之，点击否。此时，请浏览到您的MinGW C路径，选择gcc.exe文件。

#### 添加C++的编译器 ####
点击右侧“添加旁边的小箭头”选择MinGW菜单下的C++。若VijosNT-Mini能找到编译器路径，将弹出窗口询问编译器路径是否正确。如该路径正确，点击是；反之，点击否。此时，请浏览到您的MinGW C++路径，选择g++.exe文件。

#### 添加Free Pascal的编译器 ####
点击右侧“添加旁边的小箭头”选择Free Pascal。若VijosNT-Mini能找到编译器路径，将弹出窗口询问编译器路径是否正确。如该路径正确，点击是；反之，点击否。此时，请浏览到您的Free Pascal 路径，选择fpc.exe文件。
使用Free Pascal，必须保证您的编译器是32位或64位版本，而非16位版本。Free Pascal for NOI是16位版本，不能在VijosNT-Mini中使用。

### 不被列出的编译器或修改编译器设置 ###
**以下以BAT为例**

点击右侧“添加”，再点击右侧列表中新添加的一行，在扩展名匹配中输入该语言源代码的扩展名，如.bat;.cmd也可以使用通配符，如.`*`或.1?2。

点击右侧程序路径右侧的框，出现一个小按钮，点击它，浏览到您编译器的路径，如D:\bat2exe.exe。

在命令行右侧的框里输入编译命令行，如bat2exe –bat input.bat –exe output.exe。

点击环境变量右侧的框，输入需要修改或添加的环境变量，此时，编译器以及运行的程序都只识别此环境变量，如path=D:\batshells\;%path%，多个环境变量用“;”分割。

点击编译时间右侧的框，修改编译器编译时间上限，本项设置太小可能导致编译失败，太大可能导致被恶意代码卡机。此设置单位为毫秒。

内存配额处可填入编译器使用的最大内存，以防止恶意代码的攻击。

活动进程数配额中输入编译器最大的活动进程数。

源文件名处输入在编译命令行中指定的源文件名，如input.bat

目标文件名处输入在编译命令行中指定的输出文件名，如output.exe

### 不被列出的解释器或修改解释器设置 ###
**以下以BAT为例**

在右侧将源文件名、目标文件名填写一致，如Test.bat，不填写程序路径及命令行，其他按上面“不被列出的编译器或修改编译器设置”中进行设置。

在右侧执行设置下的“目标程序路径”中输入解释器路径，如C:\Windows\system32\cmd.exe，在目标命令行中输入命令行信息，如cmd /c Test.bat。

### 不被列出的编译再解释语言 ###
按上面“不被列出的编译器或修改编译器设置”中进行设置。但输出设定为待解释文件。再按“不被列出的解释器或修改解释器设置”进行设置。

### 最后配置 ###
选择编译器，点击“上移”或“下移”修改编译器匹配顺序，编译器是自上到下匹配，这样可以提高效率，如果有使用通配符的重复现象，如第一个编译器后缀名为.1?3，第二个编译器后缀名为.12?此时需要考虑.123属于哪种编译器，当然，也可以新增一种编译器，后缀名设定为.123排在两者之前。

您可以点击移除，以移除一个编译器信息。

编译器配置完毕，点击“应用”将输入的设置应用到服务。

## 配置测试数据 ##
目前VijosNT Mini支持两种数据格式，数据格式暂不可自定义。这两种数据格式是:

APlusB 无参数，A+B题目

Vijos 参数为题目目录路径，Vijos数据格式

点击左边的数据集映射，点击右边“添加旁边的小箭头”，选择A+B，添加了一道A+B题目，文件名匹配为”A+B”，则文件名必须为A+B编译器后缀，例如A+B.cpp才能被转到该题，此处同样支持后缀名，并自上到下匹配。多个文件名可使用“;”分割，如A+B;APlusB。

### 标准Vijos数据格式 ###
Vijos题目目录

Input\

Output\

Config.ini

Input目录下存放输入数据，Output下存放输出数据。

Config.ini格式:

测试点数量

输入文件|输出文件|时限(秒)|分值|错误提示

输入文件|输出文件|时限(秒)|分值|错误提示

输入文件|输出文件|时限(秒)|分值|错误提示

输入文件|输出文件|时限(秒)|分值|错误提示

输入文件|输出文件|时限(秒)|分值|错误提示

……

例如

---


10

input0.txt|output0.txt|1|10|在这里我们可以写上测试点的提示信息

input1.txt|output1.txt|1|10|这里是第二个测试点的提示信息

input2.txt|output2.txt|1|10|第三个点

input3.txt|output3.txt|1|10|当然

input4.txt|output4.txt|1|10|我们也可以不写

input5.txt|output5.txt|1|10|可以留空

input6.txt|output6.txt|1|10|也就像下面这样

input7.txt|output7.txt|1|10|

input8.txt|output8.txt|1|10|

input9.txt|output9.txt|1|10|


---

您可以点击移除以删除一个数据集。

数据集设置完成，点击“应用”。

## 执行设置 ##

此设置用于进行执行时的线程数和安全等级等信息。目前只能设置线程数。

您可以在“并发数”右边的框中输入测评线程数。

注意：Free Pascal编译器在多线程的时候可能发生不稳定的Compile Error情况。

执行设置完成，点击“应用”

## 数据源设置 ##

此处用于与SQL数据库连接，目前没有功能可用。

## 进行测评 ##

将待测评的文件拖放到VijosNT-Mini悬浮窗上进行测评，拖上去后，悬浮窗变蓝，此时正在测评，打开控制台，点击左边的VijosNT，右边就会有测评结果，双击测评队列中的一个记录，将显示详细测评结果。

## 测评结果 ##

测评完成后，悬浮窗变回绿色，同时返回测评信息

None目前没有结果，可能没有打开测评服务或等待测评。

Accepted测试通过，所有测试点正确。

WrongAnswer答案错误，至少一个测试点出现了输出错误。

RuntimeError运行是错误，至少一个测试点出现了运行时错误。

CompileError编译错误，程序不能通过编译，请检查程序和编译器设置。

TestSuiteNotFound找不到测试数据，请检查测试数据设置和程序文件名。

TimeLimitExceeded超时，至少一个测试点超出时间限制。

MemoryLimitExceeded超内存，至少一个测试点超出内存限制。

JudgerError测评系统错误。

InternalError内部错误。

CompilerNotFound找不到编译器，请检查编译器设置及目录权限。

## 卸载VijosNT-Mini ##

当您不想使用VijosNT-Mini时，可以卸载VijosNT-Mini以节省系统资源，在控制台界面中，点击左边VijosNT，点击右边“停止”，即可卸载系统服务，此时可以方便的删除VijosNT-Mini。

## FAQ ##

Q:为什么我配置了编译器和数据集但还是找不到编译器好题目?

A:每项设置完毕后都需要点击"应用", 如果发现VijosNT-Mini运行不正常, 请先看看是不是每项都点了"应用"。

Q:为什么我的VijosNT Service服务无法启动?

A:可能由于您数据集配置错误或测评系统测评时崩溃。若是前者，请修改数据集配置；若是后者，请手动删除VijosNT Service服务，无法删除请重启。

Q:为什么我的测评速度这么慢?

A:可能由于您的计算机配置较低，运行速度较缓慢。您可以尝试更换硬件，清理垃圾，关闭杀毒软件等占资源较多的软件。

Q:为什么我点击“启动控制台”之后没有反应?

A:可能是您的电脑死机或是您已经打开了控制台并最小化了窗口。

Q:现在VijosNT Mini处于什么状况，可以使用了吗?

A:现在已经可以完成测评工作，但是功能部分目前没有写完，数据源连接和安全部分没有写，也存在较多的漏洞。

Q:VijosNT-Mini的图标是什么东西? 黑黑的一坨, 好像XXX一样。

A:那是我们伟大的VijosNT-Mini作者——图灵转世的能闭着眼睛写程序的Windows 7首个安全漏洞发现者之一[iceboy](http://hi.baidu.com/iceboy_)童鞋的专用头像! 严禁侮辱! 轻者重打50大板, 重者杀头!

VijosNT Mini使用帮助 Written by BML,twd2