<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:template match="/TestSuite">
    <html>
      <head>
        <style>
          body {font-family:Trebuchet MS; font-size:10px;}
          td {text-align:left;font-family:Calibri; font-size:12px;}
          th {background-color:#FC975B;color:white;font-family:Calibri;font-size:16px;}
          td.Center {text-align:Center;}
          pre {border: 1px solid #000000;background-color:#fdb78d;color:black;font-family:Calibri; valign:Center; padding: 5px 5px 5px 5px}
          td.Pass {text-align:Center;color:Green;font-weight:bold}
          td.Fail {text-align:Center;color:Red;font-weight:bold}
          td.Test {text-align:left;}
          td.LeftAlign {text-align:left;}
          pre {word-wrap:break-word}
        </style>
        <h1 align="center">AVOCET Automation - Test Execution Results</h1>
      </head>

      <body>

        
        <h2>Test Setup Details</h2>
        <table border="1" width="40%">
          <tr bgcolor="#9acd32">
            <th>Product</th>
            <th>Version</th>
            <th>Baseline</th>
            <th>Configuration</th>
          </tr>
          <tr>
            <td>
              <xsl:value-of select="(properties/property)[1]/@value"/>
            </td>
            <td>
              <xsl:value-of select="(properties/property)[2]/@value"/>
            </td>
            <td>
              <xsl:value-of select="(properties/property)[3]/@value"/>
            </td>
            <td class='Center'>
              <xsl:value-of select="configuration/@name"/>
            </td>
          </tr>
        </table>


       
        
        

        <h2>Test Case Results</h2>

        <table id="TestResultsTable" border="1">
          <tr bgcolor="#9acd32">
            <th width="25px">Sr.no.</th>
            <th width="50px">ID</th>
            <th width="500px">Test Case Title</th>
            <th width="125px">Test Result</th>
            
          </tr>
          <xsl:for-each select="testcase">
            <tr style='display:table-row'>
              <td>
                <xsl:value-of select="1 + count(preceding-sibling::testcase)" />
              </td>
              <td class="LeftAlign">
                <xsl:value-of select="@ID"/>
              </td>
              <td class="LeftAlign">

                <xsl:value-of select="@name"/>
                
              </td>
              <xsl:variable name="status" select="@result" />
              <xsl:choose>
                <xsl:when test="$status='Passed'">
                  <td class="Pass">Pass</td>
                </xsl:when>
                <xsl:otherwise>
                  <td class="Fail">Fail</td>
                </xsl:otherwise>
              </xsl:choose>
              
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

          function showHideTable(divname)
          {
          //alert(divname);
          var result = divname.split("_",1);

          var modl = divname.split("_");
          var rescount;
          rescount = modl[1];
          
          //alert(result);
          //alert(module[1]);
          //alert(rescount);

          if(rescount != 0)
          {
          var tblObj = document.getElementById('TestResultsTable');
          var no_of_rows = tblObj.rows.length;
          //alert('rows are ' + no_of_rows);
          var tempres;
          if(result=='Pass')
          tempres='Fail';
          else
          {
          if(result=='Fail')
          tempres='Pass';
          else
          tempres = result;
          }

          //alert('ser value is - '+tempres)
          <xsl:comment>
            <![CDATA[
                  for(var i=0; i<no_of_rows;i++)
                  {
                    tblObj.rows[i].style.display = "";
                  }
          
                  for(var i=0; i<no_of_rows;i++)
                  {
                    if(tblObj.rows[i].cells[1].innerHTML == 'All' && i!=0 && module[1] == 'All')
                    {  
                      tblObj.rows[i].style.display = 'none';
                    }
                    else
                    {
                      if(tblObj.rows[i].cells[3].innerHTML == tempres)
                      tblObj.rows[i].style.display = 'none';
                    }
	        		    }
                  var count = 1;
                  for(var k=0; k < no_of_rows; k++)
                  {
                    if(tblObj.rows[k].style.display != 'none' && k!=0)
                    {
                      tblObj.rows[k].cells[0].innerHTML = count;
                      count ++;
                    }
                  } 
        
                  var arrayOfDivs = document.getElementsByTagName('div');
                  //alert(arrayOfDivs.length);
                  for(var i = 0; i < arrayOfDivs.length; i++) {        
                    if(arrayOfDivs[i].style.display =='block')
                    arrayOfDivs[i].style.display = 'none';
                  }
                ]]>
          </xsl:comment>
          }
          } // End of Function showHideTable
          
          
        </script>
        <div id="chart_div" style="position:absolute; width: 360px; height: 180px; top: 100px; right: 25px;">
        </div>
        
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
