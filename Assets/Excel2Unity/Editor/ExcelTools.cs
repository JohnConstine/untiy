using System;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityGameFramework.Runtime;
using Object = UnityEngine.Object;

namespace SG1.Editor
{
	public class ExcelTools : EditorWindow
	{
		/// <summary>
		/// 当前编辑器窗口实例
		/// </summary>
		private static ExcelTools m_Instance;

		/// <summary>
		/// Excel文件列表
		/// </summary>
		private static List<string> m_ExcelList;

		/// <summary>
		/// 项目根路径	
		/// </summary>
		private static string m_PathRoot;

		/// <summary>
		/// 滚动窗口初始位置
		/// </summary>
		private static Vector2 m_ScrollPos;

		/// <summary>
		/// 输出格式索引
		/// </summary>
		private static int m_IndexOfFormat = 0;

		/// <summary>
		/// 输出格式
		/// </summary>
		private static string[] m_FormatOption = new string[] {"TXT", "JSON", "CSV", "XML", "LUA"};

		/// <summary>
		/// 编码索引
		/// </summary>
		private static int m_IndexOfEncoding = 0;

		/// <summary>
		/// 编码选项
		/// </summary>
		private static string[] m_EncodingOption = new string[] {"UTF-8", "GB2312"};

		/// <summary>
		/// 是否保留原始文件
		/// </summary>
		private static bool m_KeepSource = true;

		/// <summary>
		/// 显示当前窗口	
		/// </summary>
		[MenuItem(Constant.AssemblyInfo.Namespace + "/ExcelTools")]
		static void ShowExcelTools()
		{
			Init();
			//加载Excel文件
			LoadExcel();
			m_Instance.Show();
		}

		void OnGUI()
		{
			DrawOptions();
			DrawExport();
		}

		/// <summary>
		/// 绘制插件界面配置项
		/// </summary>
		private void DrawOptions()
		{
			GUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("请选择格式类型:", GUILayout.Width(85));
			m_IndexOfFormat = EditorGUILayout.Popup(m_IndexOfFormat, m_FormatOption, GUILayout.Width(125));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("请选择编码类型:", GUILayout.Width(85));
			m_IndexOfEncoding = EditorGUILayout.Popup(m_IndexOfEncoding, m_EncodingOption, GUILayout.Width(125));
			GUILayout.EndHorizontal();

			m_KeepSource = GUILayout.Toggle(m_KeepSource, "保留Excel源文件");
		}

		/// <summary>
		/// 绘制插件界面输出项
		/// </summary>
		private void DrawExport()
		{

				if (m_ExcelList == null) return;
				if (m_ExcelList.Count < 1)
				{
					EditorGUILayout.LabelField("目前没有Excel文件被选中哦!");
				}
				else
				{
					EditorGUILayout.LabelField("下列项目将被转换为" + m_FormatOption[m_IndexOfFormat] + ":");
					GUILayout.BeginVertical();
					m_ScrollPos = GUILayout.BeginScrollView(m_ScrollPos, false, true, GUILayout.Height(150));
					foreach (string s in m_ExcelList)
					{
						GUILayout.BeginHorizontal();
						GUILayout.Toggle(true, s);
						GUILayout.EndHorizontal();
					}

					GUILayout.EndScrollView();
					GUILayout.EndVertical();

					//输出
					if (GUILayout.Button("转换"))
					{
						Convert();
					}
				}

		}

		/// <summary>
		/// 转换Excel文件
		/// </summary>
		private static void Convert()
		{
			foreach (string assetsPath in m_ExcelList)
			{
				//获取Excel文件的绝对路径
				string excelPath = m_PathRoot + "/" + assetsPath;

				//构造Excel工具类
				ExcelUtility excel = new ExcelUtility(excelPath);

				//判断编码类型
				Encoding encoding = null;
				if (m_IndexOfEncoding == 0 || m_IndexOfEncoding == 3)
				{
					encoding = Encoding.GetEncoding("utf-8");
				}
				else if (m_IndexOfEncoding == 1)
				{
					encoding = Encoding.GetEncoding("gb2312");
				}

				//判断输出类型
				string output = "";
				switch (m_IndexOfFormat)
				{
					case 1:
						output = excelPath.Replace(".xlsx", ".json");
						excel.ConvertToJson(output, encoding);
						break;
					case 2:
						output = excelPath.Replace(".xlsx", ".csv");
						excel.ConvertToCSV(output, encoding);
						break;
					case 3:
						output = excelPath.Replace(".xlsx", ".xml");
						excel.ConvertToXml(output);
						break;
					case 4:
						output = excelPath.Replace(".xlsx", ".lua");
						excel.ConvertToLua(output, encoding);
						break;
					case 0:
						output = excelPath.Replace(".xlsx", ".txt");
						excel.ConvertToTxt(output, encoding);
						break;
				}

				//判断是否保留源文件
				if (!m_KeepSource)
				{
					FileUtil.DeleteFileOrDirectory(excelPath);
				}

				//刷新本地资源
				AssetDatabase.Refresh();
			}

			//转换完后关闭插件
			//这样做是为了解决窗口
			//再次点击时路径错误的Bug
			m_Instance.Close();
		}

		/// <summary>
		/// 加载Excel
		/// </summary>
		private static void LoadExcel()
		{
			if (m_ExcelList == null) m_ExcelList = new List<string>();
			m_ExcelList.Clear();
			//获取选中的对象
			object[] selection = (object[]) Selection.objects;
			//判断是否有对象被选中
			if (selection.Length == 0)
				return;
			//遍历每一个对象判断不是Excel文件
			foreach (Object obj in selection)
			{
				string objPath = AssetDatabase.GetAssetPath(obj);
				if (objPath.EndsWith(".xlsx"))
				{
					m_ExcelList.Add(objPath);
				}
			}
		}

		private static void Init()
		{
			//获取当前实例
			m_Instance = EditorWindow.GetWindow<ExcelTools>();
			//初始化
			m_PathRoot = Application.dataPath;
			//注意这里需要对路径进行处理
			//目的是去除Assets这部分字符以获取项目目录
			//我表示Windows的/符号一直没有搞懂
			m_PathRoot = m_PathRoot.Substring(0, m_PathRoot.LastIndexOf("/"));
			m_ExcelList = new List<string>();
			m_ScrollPos = new Vector2(m_Instance.position.x, m_Instance.position.y + 75);
		}

		void OnSelectionChange()
		{
			//当选择发生变化时重绘窗体
			Show();
			LoadExcel();
			Repaint();
		}
	}
}

