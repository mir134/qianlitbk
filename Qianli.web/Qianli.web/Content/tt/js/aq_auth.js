!function(f,e){function h(){this.hookId="aqLogoHook",this.types={business:"hy",realname:"sm",official:"gw",enterprise:"qy"}}h.prototype.$id=function(b){return document.getElementById(b)},h.prototype.getInfo=function(j){var i=this,o=j.getAttribute("logo_type"),n=j.getAttribute("logo_size"),m=n.split("x"),l=i.types[o],k={w:m[0],h:m[1],sizeInfo:n,typeInfo:o,type:l};return k},h.prototype.getUrlByInfo=function(b){var j=e.location.host,i="http://v.pinpaibao.com.cn/authenticate/cert/?site="+j+"&at="+b.typeInfo;return i},h.prototype.getImgByInfo=function(b){var d='<img src="other/'+b.type+"_"+b.sizeInfo+'.png" alt="\u5b89\u5168\u8054\u76df\u8ba4\u8bc1" width="'+b.w+'" height="'+b.h+'" style="border: none;">';return d},h.prototype.init=function(){var b=this;e.write('<span id="'+b.hookId+'" style="display: none;"></span>');var m=b.$id(b.hookId),l=m.parentNode,k=b.getInfo(l),j=b.getImgByInfo(k),i=b.getUrlByInfo(k);l.setAttribute("href",i),l.setAttribute("target","_blank"),e.write(j)};var g=new h;g.init()}(window,document);