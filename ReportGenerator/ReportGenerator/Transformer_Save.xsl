<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:template match="/ArrayOfTestResults">
    <html>
      <head>
        <style>
          body {font-family:Trebuchet MS; font-size:10px;}
          td {text-align:left;font-family:Calibri; font-size:12px;}
          th {background-color:#FC975B;color:white;font-family:Calibri;font-size:16px;}
          td.Center {text-align:Center;}
          pre {border: 1px solid #000000;background-color:#fdb78d;color:black;font-family:Calibri; valign:Center; padding: 5px 5px 5px 5px}
          td.Pass {color:Green;font-weight:bold;text-align:Center;}
          td.Fail {color:Red;font-weight:bold;text-align:Center;}
          pre {word-wrap:break-word}
        </style>
      </head>
      <body onload="initButton()">

        <span id="topButton"
        style="position: absolute; visibility: hidden; bottom: 15px; left: 10px; width: 74px;">
          <a href="javascript: window.scrollTo(0, 0); void 0" onmouseover="window.status = 'Top'; return true;" onmouseout="window.status = ''; return true;">Back to Top</a>
        </span>

        <img src=".\background.jpg" alt="Synechron Test Engineering Program" title="Synechron Test Expedition Platform" border="0" />
        <h2>Test Environment</h2>
        <table border="1" width="40%">
          <tr bgcolor="#9acd32">
            <th>OS Name</th>
            <th>Machine Name</th>
            <th>ProjectName</th>
            <th>ProjectVersion</th>
          </tr>
          <tr>
            <td>
              <xsl:value-of select="TestResults/OS"/>
            </td>
            <td>
              <xsl:value-of select="TestResults/machineName"/>
            </td>
            <td>
              <xsl:value-of select="TestResults/projectName"/>
            </td>
            <td class='Center'>
              <xsl:value-of select="TestResults/projectVersion"/>
            </td>
          </tr>
        </table>
        <h2>Suite Execution Summary</h2>
        <div>
          <table border="1">
            <tr bgcolor="#9acd32">
              <th>Sr.No.</th>
              <th>Module</th>
              <th>Passed</th>
              <th>Failed</th>
              <th>Total</th>
              <th>ExecutionTime</th>
            </tr>
            <xsl:variable name="unique-list" select="//suiteName[not(.=following::suiteName)]" />
            <xsl:for-each select="$unique-list">
              <xsl:variable name="module_Name" select="."/>
              <xsl:variable name="TC" select="count(/ArrayOfTestResults/TestResults[suiteName=$module_Name])"/>
              <xsl:variable name="PC" select="count(/ArrayOfTestResults/TestResults[testCaseResult='Pass'][suiteName=$module_Name])"/>
              <xsl:variable name="FC" select="count(/ArrayOfTestResults/TestResults[testCaseResult='Fail'][suiteName=$module_Name])"/>
              <xsl:variable name="ET" select="sum(/ArrayOfTestResults/TestResults[suiteName=$module_Name]/executionTime)"/>
              <tr>
                <td class='Center'>
                  <xsl:value-of select="position()" />
                </td>
                <td>
                  <xsl:value-of select="$module_Name"/>
                </td>
                <td class='Center'>
                  <xsl:value-of select='$PC'/>
                </td>
                <td class='Center'>
                  <xsl:value-of select='$FC'/>
                </td>
                <td class='Center'>
                  <xsl:value-of select='$TC'/>
                </td>
                <td class='Center'>
                  <xsl:value-of select="floor($ET div 60)"/>m<xsl:value-of select="format-number($ET mod 60,'#')"/>s
                </td>
              </tr>
            </xsl:for-each>
            <tr bgcolor='#fdb78d'>
              <td colspan='2' class='Center'>
                <b>Grand Total</b>
              </td>
              <td id="Pass" class='Center'>
                <b>
                  <a href='javascript:return 0;'>
                    <xsl:attribute name="onclick">
                      <xsl:text>showHideTR('Pass')</xsl:text>
                    </xsl:attribute>
                    <xsl:value-of select='count(TestResults[testCaseResult="Pass"])'/>
                  </a>
                </b>
              </td>
              <td id="Fail" class='Center'>
                <b>
                  <a href='javascript:return 0;'>
                    <xsl:attribute name="onclick">
                      <xsl:text>showHideTR('Fail')</xsl:text>
                    </xsl:attribute>
                    <xsl:value-of select='count(TestResults[testCaseResult="Fail"])'/>
                  </a>
                </b>
              </td>
              <td id="Total" class='Center'>
                <b>
                  <a href='javascript:return 0;'>
                    <xsl:attribute name="onclick">
                      <xsl:text>showHideTR('Total')</xsl:text>
                    </xsl:attribute>
                    <xsl:value-of select='count(TestResults[testCaseResult="Pass"]) + count(TestResults[testCaseResult="Fail"])'/>
                  </a>
                </b>
              </td>
              <td id="ExecutionTime" class='Center'>
                <xsl:variable name="GET" select='sum(/ArrayOfTestResults/TestResults/executionTime)'/>
                <b>
                  <xsl:value-of select="floor($GET div 60)"/>m<xsl:value-of select="format-number($GET mod 60,'#')"/>s
                </b>
              </td>
            </tr>
          </table>
          <br/>

          <table border="1" style="position: relative; top: 0px; left: 0px;">
            <tr bgcolor="#9acd32">
              <th>Execution Date</th>
              <th>Execution StartTime</th>
              <th>Execution EndTime</th>
              <th>ExecutionTime</th>
            </tr>
            <tr>
              <td class='Center'>
                <xsl:value-of select="substring-before(TestResults/startTime,'T')"/>
              </td>
              <td class='Center'>
                <xsl:value-of select="substring-after(TestResults/startTime,'T')"/>
              </td>
              <td class='Center'>
                <xsl:value-of select="substring-after(TestResults[last()]/endTime,'T')"/>
              </td>
              <td class='Center'>
                <xsl:value-of select="floor(sum(TestResults/executionTime) div 60)"/>m<xsl:value-of select="format-number((sum(TestResults/executionTime)) mod 60,'#')"/>s
              </td>
            </tr>
          </table>

        </div>
        <h2>Test Results</h2>

        <table border="1">
          <tr bgcolor="#9acd32">
            <th width="25px">Sr.no.</th>
            <th width="50px">Suite</th>
            <th width="500px">Test Case</th>
            <th width="125px">Test Result</th>
            <th width="75px">Time</th>
          </tr>
          <xsl:for-each select="TestResults">
            <xsl:variable name="status" select="testCaseResult" />
            <tr class= "{$status}">
              <td class='Center'>
                <xsl:value-of select="1 + count(preceding-sibling::TestResults)" />
              </td>
              <td>
                <xsl:value-of select="suiteName"/>
              </td>
              <td>
                <a href='javascript:return 0;'>
                  <xsl:attribute name="onclick">
                    <xsl:text>showHideDiv('</xsl:text>
                    <xsl:value-of select="testCaseName"/>
                    <xsl:value-of select="testCaseTitle"/>
                    <xsl:text>')</xsl:text>
                  </xsl:attribute>
                  <xsl:value-of select="testCaseName"/>
                  <xsl:value-of select="testCaseTitle"/>
                </a>
                <div style='display:none'>
                  <xsl:attribute name="id">
                    <xsl:value-of select="testCaseName"/>
                    <xsl:value-of select="testCaseTitle"/>
                  </xsl:attribute>
                  <xsl:variable name="TCNAME" select='testCaseName'/>
                  <pre id="{$TCNAME}">
                    <xsl:value-of select="testSteps"/>
                  </pre>
                  <b>
                    <a style="MARGIN-TOP: -3%; MARGIN-BOTTOM: -3%; FLOAT: right" href = "#{$TCNAME}">
                      Top of Test Case
                    </a>
                  </b>
                </div>
              </td>
              <td class= "{$status}">
                <xsl:value-of select="testCaseResult" />
              </td>
              <td class='Center'>
                <xsl:value-of select="floor(executionTime div 60)"/>m<xsl:value-of select="format-number(executionTime mod 60,'#')"/>s
              </td>
            </tr>
          </xsl:for-each>
        </table>

        <script type="text/javascript" src="https://www.google.com/jsapi">
          <xsl:text>document.write("hello")</xsl:text>
        </script>

        <script type="text/javascript">
          google.load("visualization", "1", {packages:["corechart"]});
          google.setOnLoadCallback(drawChart);
          function drawChart() {
          var pass=document.getElementById('Pass').innerText;
          var fail=document.getElementById('Fail').innerText;
          var data = new google.visualization.DataTable();
          data.addColumn('string', 'Status');
          data.addColumn('number', 'Pass');
          data.addColumn('number', 'Fail');
          data.addRows(3);
          data.setValue(0, 0, 'Status');
          data.setValue(0, 1, parseInt(pass));
          data.setValue(0, 2, parseInt(fail));
          var chart = new google.visualization.ColumnChart(document.getElementById('chart_div'));
          chart.draw(data, {width: 400, height: 200, title: 'Test Results', legend:'bottom', hAxis: {title:'Status'},
          series: {0:{color: '4D8915', visibleInLegend: true}, 1:{color: 'FF9900', visibleInLegend: true}, 2:{color: 'C3D3B9', visibleInLegend: true}},
          vAxis: {title: 'No. of Tests', titleTextStyle: {color: 'black'}}});
          }

          function showHideDiv(divId)
          {
          var divstyle;divstyle = document.getElementById(divId).style.display;
          if(divstyle.toLowerCase()=='none')
          {document.getElementById(divId).style.display = 'block';}
          else{document.getElementById(divId).style.display = 'none';}
          }

          function showHideTR(lnkId)
          {
          var tds = document.getElementsByTagName("tr");
          var size = tds.length;
          <xsl:comment>
            <![CDATA[
				        for(var j=0; j < size; j++){
					        tds[j].style.display = "";
				        }	
				        if(lnkId != "Total"){
					        for(var i=0; i < size; i++){
						        var classNameVal = tds[i].className;
						        if(classNameVal != ""){					
  							      if(classNameVal != lnkId){
	  							      tds[i].style.display = "none";
							        }	
						        }
					        }
				        }			
			        ]]>
          </xsl:comment>
          }

          function floatButton() {
          if (document.all) {
          //alert(document.body.offsetHeight)
          document.all.topButton.style.pixelTop = document.body.scrollTop + document.body.offsetHeight - 30;
          }
          else if (document.layers) {
          document.topButton.top = window.pageYOffset;
          }
          else if (document.getElementById) {
          document.getElementById('topButton').style.top = window.pageYOffset + 'px';
          }
          }

          if (document.all)
          window.onscroll = floatButton;
          else
          setInterval('floatButton()', 100);

          function initButton() {
          if (document.all) {
          document.all.topButton.style.pixelLeft = document.body.clientWidth - document.all.topButton.offsetWidth;
          document.all.topButton.style.visibility = 'visible';
          }
          else if (document.layers) {
          document.topButton.left = window.innerWidth - document.topButton.clip.width - 15;
          document.topButton.visibility = 'show';
          }
          else if (document.getElementById) {
          document.getElementById('topButton').style.left = (window.innerWidth - 35) + 'px';
          document.getElementById('topButton').style.visibility = 'visible';
          }
          }

        </script>
        <div id="chart_div" style="position:absolute; width: 360px; height: 180px; top: 100px; right: 25px;">
        </div>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>