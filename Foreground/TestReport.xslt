<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="html" />

  <xsl:template match="/TestResult">
    <html>
      <body>
        <!-- TODO -->
        <h2>测试报告</h2>
        序号: <xsl:value-of select="Id"/><br />
        文件名: <xsl:value-of select="FileName"/><br />
        日期: <xsl:value-of select="Date"/><br />
        状态: <xsl:value-of select="Flag"/><br />
        分数: <xsl:value-of select="Score"/><br />
        时间: <xsl:value-of select="TimeUsage"/><br />
        内存: <xsl:value-of select="MemoryUsage"/><br />
        警告: <xsl:value-of select="Warning"/><br />
        <table>
          <tr>
            <td>序号</td>
            <td>状态</td>
            <td>分数</td>
            <td>时间</td>
            <td>内存</td>
            <td>警告</td>
          </tr>
          <xsl:for-each select="TestEntry">
            <tr>
              <td>
                <xsl:value-of select="Index"/>
              </td>
              <td>
                <xsl:value-of select="Flag"/>
              </td>
              <td>
                <xsl:value-of select="Score"/>
              </td>
              <td>
                <xsl:value-of select="TimeUsage"/>
              </td>
              <td>
                <xsl:value-of select="MemoryUsage"/>
              </td>
              <td>
                <xsl:value-of select="Warning"/>
              </td>
            </tr>
          </xsl:for-each>
        </table>
        <textarea cols="80" rows="20">
          <xsl:value-of select="SourceCode"/>
        </textarea>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
