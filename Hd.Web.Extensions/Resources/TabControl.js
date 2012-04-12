      function showTabIndex(clientTabId,serverTabId,tabIndex,selectedTabCssClass,TabCssClass)
      {
        
          var uxTabControl = $get(clientTabId); 
          var tabs =  uxTabControl.childNodes[1].getElementsByTagName("LI"); 
          for(var index=0; index<tabs.length; index++)
          {   
                tabs[index].className = tabs[index].attributes["tabIndexNumber"].value == tabIndex 
                    ? selectedTabCssClass
                    : TabCssClass;
                  
          } 
          var tabsContents = uxTabControl.childNodes[2].childNodes;
          var selectedTabContent = null;
          for(var index=0; index<tabsContents.length; index++)
          {
             if(tabsContents[index].nodeType==3)
                 continue;
             if(tabsContents[index].attributes["tabIndexNumber"].value==tabIndex)
             {
                 tabsContents[index].style.display = 'block';  
                 selectedTabContent  = tabsContents[index];                
             }
             else
             {
                tabsContents[index].style.display ='none';
             }
          }
         
          if(selectedTabContent.childNodes.length==0||(selectedTabContent.childNodes.length==1&&selectedTabContent.childNodes[0].nodeType==3))
           __doPostBack(serverTabId,"L:"+tabIndex); 
          else
           __doPostBack(serverTabId,"S:"+tabIndex);
           
          
          
          
      }