  var attachments_count = 0;
    var busyIndexes = new Array();
    
    Array.prototype.removeItem = function(s)
    {
         for(i = 0; i < this.length; i++)
         {
            if(s == this[i])
                this.splice(i, 1);
         }
    }
    
    function GetNextAvailableIndex()
    {
        var isBusy = 0;
        
        for(i = 0; i < max_attachments_count; i ++)
        {
            isBusy = 0;
            for(j = 0; j < busyIndexes.length; j ++)
            {
                if (busyIndexes[j] == i)
                    isBusy = 1;
            }
            
            if (isBusy == 0)
                return i;
        }
        
        return -1;
    }
    
    function OnGetFocusForFileDescription(e)
    {
        if (e.target.value == 'type file description')
            e.target.value = '';
    }
    
	//add file attachment form and associated elements
	function AddAttachment()
	{
	    var new_path_label = document.createElement('span');
	    var index = GetNextAvailableIndex();
	    
	    new_path_label.setAttribute('id','child_attachment_path_label_' + index);
	    document.getElementById('attachments_content').appendChild(new_path_label);
	    
		//create new <input> element
		var new_attachment = document.createElement('input');
		
		if (document.getElementById(fileUploads[index]) != null)
		    new_attachment = document.getElementById(fileUploads[index]);
		
		new_attachment.style.display = "";
		
		//give element an id
		new_attachment.setAttribute('id', fileUploads[index]);
		new_attachment.setAttribute('name', fileUploadNames[index]);
		//set element type
		if (new_attachment.type != 'file') {
			new_attachment.setAttribute('type', 'file');
		}
		
		//set element size
	    new_attachment.setAttribute('size', '25');
	
		//append newly created element to <span id="content"> tree
		document.getElementById('attachments_content').appendChild(new_attachment);

		//create new <input> element for text
		var new_text = document.createElement('input');
		
		if (document.getElementById(fileDescriptions[index]) != null)
		    new_text = document.getElementById(fileDescriptions[index]);
		
		new_text.style.display = "";
		
		//give element an id
		new_text.setAttribute('id', fileDescriptions[index]);
		new_text.setAttribute('name', fileDescriptionsText[index]);
		new_text.setAttribute('value', 'type file description');
		if (new_text.type != 'text') {
			new_text.setAttribute('type', 'text');
		}
	    new_text.setAttribute('size', '20');
	    new_text.setAttribute('style', 'margin-left:10px');
        
        $addHandler(new_text, 'focus', OnGetFocusForFileDescription);
        
		//append newly created element to <span id="content"> tree
		document.getElementById('attachments_content').appendChild(new_text);
        
		//create new <span> element
		new_text = document.createElement('span');
		//give element an id
		new_text.setAttribute('id','child_attachment_text_' + index);
		//set element HTML to produce 'remove' text link
		new_text.innerHTML = '&nbsp;<a href="javascript:RemoveAttachment(' + index + ');">remove </a><br />';
		//append newly created element to <span id="content"> tree
		
		//increase the form count
		attachments_count ++;

		busyIndexes.push(index);
        
        document.getElementById('attachments_content').appendChild(new_text);
        document.getElementById('attachments_more').innerHTML = 'Attach another file';
    
        if (max_attachments_count == attachments_count)
        {
            document.getElementById('maximumExceeded').innerHTML = "The attachments count is limited to " + max_attachments_count;
            document.getElementById('attachments_more').style.display = 'none';
        }
        InsureSubmitButton();
	}

    function ProcessAttachment(new_attachment, new_path_label, new_text)
    {
        new_path_label.innerHTML = new_attachment.value;
        
        if (new_attachment.value != '')
        	document.getElementById('attachments_content').appendChild(new_text);
            
        if (new_attachment.value != '')
            document.getElementById('attachments_more').innerHTML = 'Attach another file';
        
        if (new_attachment.value == '')
        {
            attachments_count--;
            
            document.getElementById('attachments_content').removeChild(new_attachment);
            document.getElementById('attachments_content').removeChild(new_path_label);
            
            if (attachments_count == 0)
        	    document.getElementById('attachments_more').innerHTML = 'Attach a file';
        }
    }

	//remove file attachment form and associated elements
	function RemoveAttachment(remove_form_num)
	{
		//decrease the form count
		attachments_count--;
		
		//remove <input> element attachment
		document.getElementById('attachments_content').removeChild(document.getElementById(fileUploads[remove_form_num]));
		//remove <span> element text
		document.getElementById('attachments_content').removeChild(document.getElementById('child_attachment_text_' + remove_form_num));
		//remove <span> element path
		document.getElementById('attachments_content').removeChild(document.getElementById('child_attachment_path_label_' + remove_form_num));
		
		document.getElementById('attachments_content').removeChild(document.getElementById(fileDescriptions[remove_form_num]));

		//if all forms are removed, change text back to "Attach a file"
		if (attachments_count == 0)
        	document.getElementById('attachments_more').innerHTML = 'Attach a file';
        
        document.getElementById('attachments_more').style.display = '';
        document.getElementById('maximumExceeded').innerHTML = '';
        
        busyIndexes.removeItem(remove_form_num);
        InsureSubmitButton();
	}
	
	function InsureSubmitButton()
	{
		var button = document.getElementById(_submitButtonID);
		if (button)
		{
			button.style.display = busyIndexes.length > 0 ? '' : 'none';
		}
	}
	
	function IsIE()
	{
	    version=0;
        if (navigator.appVersion.indexOf("MSIE")!=-1)
        {
            temp=navigator.appVersion.split("MSIE");
            version=parseFloat(temp[1]);
        }
        
        return version >= 5.5;
    }