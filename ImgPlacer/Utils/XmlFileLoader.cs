using System;
using System.IO;
using System.Xml.Linq;

namespace ImgPlacer.Utils
{
    public class XmlFileLoader
    {
        /// <summary>
        /// XML ファイルのフルパスを受け取り、XDocument として読み込む。
        /// 読み込みエラーはアプリ側が扱いやすい例外に包んでスローする。
        /// </summary>
        /// <param name="filePath">
        /// 読み込む XML ファイルのフルパス
        /// </param>
        /// <returns>
        /// ファイルから生成した XDocument
        /// </returns>
        public static XDocument LoadXml(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("XML ファイルパスが空です。", nameof(filePath));
            }

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"ファイルが存在しません: {filePath}");
            }

            try
            {
                // XML を XDocument としてロード
                var doc = XDocument.Load(filePath, LoadOptions.PreserveWhitespace);
                return doc;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"XML の読み込みに失敗しました: {filePath}", ex);
            }
        }
    }
}