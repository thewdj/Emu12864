给Beginner：

	你需要做的：
		编辑Game.cs文件
		编译并运行
	你一开始有什么：
		一个Engine对象，它包含必要的函数。
		DrawString(), DrawPixel(),GetKey()
		一个Keys枚举，给GetKey()使用。
	你要怎么做：
		class Game : GameBase
		{
		   //这里定义自己的变量

		   public override void Start()
			{
				//这里初始化变量
			}

		   public override void Loop()
			{
				//这里写绘图代码
			}
		}

对于有好奇心的人：

	Cores：所有核心的实现在这文件夹里
		Core.cs：核心代码，比如窗口加载
			里面的一个叫ExRate的常量：把宽ExRate的像素正方形作为一个"像素"
		DxDLL.cs：对DirectX的调用
		Editor.cs：进行图片转换之类，构建中。通过添加任意运行参数可以打开这个窗口
		Launcher.cs：启动器窗口
		Program.cs：应用程序的入口点
		Res.cs：字库在这里，是C51的字库
	DLLs：DirectX函数的DLL
	Resources：图标

其他：
	
	此项目是喵玉殿技术部的活动项目
	
使用させていただいたライブラリ等：
　  DX Library
　　　	Copyright (C) 2001-2014 Takumi Yamada.
