<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="html" />

  <xsl:template match="/TestResult">
    <html>
      <head>
        <style type="text/css">
          <xsl:text disable-output-escaping="yes"> 
            <![CDATA[
.code *{
	font-size:12px !important;
	text-decoration:none !important;
	font-variant:none !important;
	font-family:Consolas,Monaco,"Lucida Console" !important;
	font-style:normal !important;
}

pre.code{
	font-size:12px;
	color:black !important;
	background:#fff;
	font-family:Consolas,Monaco,"Lucida Console" !important;
	line-height:16px;
	margin:0 10px;
	padding:0 10px;
}

.code ol{border-left:8px groove #6699FF;}
.code i{color:#0062A8;}
.code u{color:black;text-decoration:none}
.code b,.code strong{color:navy;font-weight:normal}
.code em{color:navy;}
.code dfn{color:#0000FF;}
.code samp{color:#224c14;}
.code kbd{color:black;}
.code cite{color:#008000;}
.code tt{color:#336699;}
.code s,.code strike,.code del{color:purple}
table,th,td {
	border: 1px solid #CCCCCC;
}
th {
	background-color: #CCCCCC;
	border: 1px solid #CCCCCC;
}
            ]]>
          </xsl:text>
        </style>
        <script language="JavaScript" type="text/javascript">
          <xsl:text disable-output-escaping="yes"> 
            <![CDATA[
/*
ui.js

Author:		ChiChou
LastUpdate:	2009-12-05 
*/

function OnColor(stx){ //着色
	if($("code").value.length == 0){
		return;
	}
	
	var clsHightlight = new CLASS_HIGHLIGHT($("code").value,stx);
	var szCode=clsHightlight.highlight();

	$("codeFrame").innerHTML=szCode;
}

/*
Create elements
*/

function $(szID){return document.getElementById(szID);}
/**
CLASS_HIGHLIGHT
**/

function CLASS_HIGHLIGHT(code,syntax)
{
	//Hash table class
	function Hashtable(){
		this._hash = new Object();
		this.add = function(key,value){
			if(typeof(key)!="undefined"){
				if(this.contains(key)==false){
					this._hash[key]=typeof(value)=="undefined"?null:value;
					return true;
				}else{
					return false;
				}
			}
			else{
				return false;
			}
		}
		this.remove		= function(key){delete this._hash[key];}
		this.count		= function(){var i=0;for(var k in this._hash){i++;} return i;}
		this.items		= function(key){return this._hash[key];}
		this.contains	= function(key){return typeof(this._hash[key])!="undefined";}
		this.clear		= function(){for(var k in this._hash){delete this._hash[k];}}
	}

	this._caseSensitive = true;

	//Convert string 2 hashtable
	this.str2hashtable = function(key,cs){
		var _key	= key.split(/,/g);
		var _hash	= new Hashtable();
		var _cs		= true;

		if(typeof(cs)=="undefined"||cs==null){
			_cs = this._caseSensitive;
		}else{
			_cs = cs;
		}

		for(var i in _key){
			if(_cs){
				_hash.add(_key[i]);
			}else{
				_hash.add((_key[i]+"").toLowerCase());
			}
		}
		return _hash;
	}

	//Get code
	this._codetxt = code.replace(/\r\n/g,"\n");

	if(typeof(syntax)=="undefined"){
		syntax = "";
	}
	
	switch(syntax.toLowerCase()){
		case "cs": //C#
			this._caseSensitive	= true;
			this._keywords		= this.str2hashtable("abstract,as,base,bool,break,byte,case,catch,char,checked,class,const,continue,decimal,default,delegate,do,double,else,enum,event,explicit,extern,false,finally,fixed,float,for,foreach,get,goto,if,implicit,in,int,interface,internal,is,lock,long,namespace,new,null,object,operator,out,override,params,private,protected,public,readonly,ref,return,sbyte,sealed,short,sizeof,stackalloc,static,set,string,struct,switch,this,throw,true,try,typeof,uint,ulong,unchecked,unsafe,ushort,using,value,virtual,void,volatile,while");
			this._commonObjects = this.str2hashtable("String,Boolean,DateTime,Int32,Int64,Exception,DataTable,DataReader");
			this._tags			= this.str2hashtable("",false);
			this._wordDelimiters= " ,.?!;:\\/<>(){}[]\"'\r\n\t=+-|*%@#$^&";
			this._quotation		= this.str2hashtable("\"");
			this._lineComment	= "//";
			this._escape		= "\\";
			this._commentOn		= "/*";
			this._commentOff	= "*/";
			this._ignore		= "";
			this._dealTag		= false;
			break;			
		case "java":
			this._caseSensitive	= true;
			this._keywords		= this.str2hashtable("abstract,boolean,break,byte,case,catch,char,class,const,continue,default,do,double,else,extends,final,finally,float,for,goto,if,implements,import,instanceof,int,interface,long,native,new,package,private,protected,public,return,short,static,strictfp,super,switch,synchronized,this,throw,throws,transient,try,void,volatile,while");
			this._commonObjects = this.str2hashtable("String,Boolean,DateTime,Int32,Int64,Exception,DataTable,DataReader");
			this._tags			= this.str2hashtable("",false);
			this._wordDelimiters= " ,.?!;:\\/<>(){}[]\"'\r\n\t=+-|*%@#$^&";
			this._quotation		= this.str2hashtable("\"");
			this._lineComment	= "//";
			this._escape		= "\\";
			this._commentOn		= "/*";
			this._commentOff	= "*/";
			this._ignore		= "";
			this._dealTag		= false;
			break;
		case "vb":
			this._caseSensitive	= false;
			this._keywords		= this.str2hashtable("Lib,Declare,And,ByRef,ByVal,Call,Case,Class,Const,Dim,Do,Each,Else,ElseIf,Empty,End,Eqv,Erase,Error,Exit,Explicit,False,For,Get,If,Imp,In,Is,Let,Loop,Mod,Next,Not,Nothing,Null,As,New,On,Option,Or,Private,Property,Public,Randomize,ReDim,Resume,Select,Set,Step,Sub,Then,To,True,Until,Wend,While,Xor,Anchor,Array,Asc,Atn,CBool,CByte,CCur,CDate,CDbl,Chr,CInt,CLng,Cos,CreateObject,CSng,CStr,Date,DateAdd,DateDiff,DatePart,DateSerial,DateValue,Day,Dictionary,Document,Element,Err,Exp,FileSystemObject,Filter,Fix,Int,Form,FormatCurrency,FormatDateTime,FormatNumber,FormatPercent,GetObject,Hex,Hour,InputBox,InStr,InstrRev,IsArray,IsDate,IsEmpty,IsNull,IsNumeric,IsObject,Join,LBound,LCase,Left,Len,Link,LoadPicture,Location,Log,LTrim,RTrim,Trim,Mid,Minute,Month,MonthName,MsgBox,Navigator,Now,Oct,Replace,Right,Rnd,Round,Second,Sgn,Sin,Space,Split,Sqr,StrComp,StrReverse,Tan,Time,TextStream,TimeSerial,TimeValue,TypeName,UBound,UCase,VarType,Weekday,WeekDayName,Year,Typeof,Type,Enum,Input,Output,Open,Close,Line,Seek,FileAttr,GetAttr,SetAttr,FileLen,EOF,LOF,Reset,Spc,Dir,Kill,Lock,Unlock,Name,Format,Tab,FileDateTime,Put,Input,Print,FileCopy,Write,FreeFile,Loc,CallByName,Choose,CVErr,DDB,DoEvents,Environ");
			this._commonObjects = this.str2hashtable("String,Number,Boolean,Date,Integer,Long,Double,Single,Currency,Decimal,Object,Variant,Wscript,Function,ScriptEngine,ScriptEngineBuildVersion,ScriptEngineMajorVersion,ScriptEngineMinorVersion");
			this._tags		= this.str2hashtable("",false);
			this._wordDelimiters= " ,.?!;:\\/<>(){}[]\"'\r\n\t=+-|*%@#$^&";
			this._quotation		= this.str2hashtable("\"");
			this._lineComment	= "'";
			this._escape		= "";
			this._commentOn		= "";
			this._commentOff	= "";
			this._ignore		= "";
			this._dealTag		= false;
			break;			
		case "pas":
			this._caseSensitive	= false;
			this._keywords		= this.str2hashtable("abs,addr,and,ansichar,ansistring,array,as,asm,begin,boolean,byte,cardinal,case,char,class,comp,const,constructor,currency,destructor,div,do,double,downto,else,end,except,exports,extended,false,file,finalization,finally,for,function,goto,if,implementation,in,inherited,int64,initialization,integer,interface,is,label,library,longint,longword,mod,nil,not,object,of,on,or,packed,pansichar,pansistring,pchar,pcurrency,pdatetime,pextended,pint64,pointer,private,procedure,program,property,pshortstring,pstring,pvariant,pwidechar,pwidestring,protected,public,published,raise,real,real48,record,repeat,set,shl,shortint,shortstring,shr,single,smallint,string,then,threadvar,to,true,try,type,unit,until,uses,val,var,varirnt,while,widechar,widestring,with,word,write,writeln,xor");
			this._commonObjects = this.str2hashtable("");
			this._tags			= this.str2hashtable("",false);
			this._wordDelimiters= " ,.?!;:{}\\/<>()[]\"'\r\n\t=+-|*%@#$^&";
			this._quotation		= this.str2hashtable("\",'");
			this._lineComment	= "//";
			this._escape		= "\\";
			this._commentOn		= this.str2hashtable("/*");
			this._commentOff	= this.str2hashtable("*/");
			this._ignore		= "";
			break;			
		case "rb":
			this._caseSensitive	= false;
			this._keywords		= this.str2hashtable("alias,and,BEGIN,begin,break,case,class,def,define_method,defined,do,each,else,elsif,END,end,ensure,false,for,if,in,module,new,next,nil,not,or,raise,redo,rescue,retry,return,self,super,then,throw,true,undef,unless,until,when,while,yield");
			this._commonObjects = this.str2hashtable("Array,Bignum,Binding,Class,Continuation,Dir,Exception,FalseClass,File::Stat,File,Fixnum,Fload,Hash,Integer,IO,MatchData,Method,Module,NilClass,Numeric,Object,Proc,Range,Regexp,String,Struct::TMS,Symbol,ThreadGroup,Thread,Time,TrueClass");
			this._tags			= this.str2hashtable("",false);
			this._wordDelimiters= " ,.?!;:\\/<>(){}[]\"'\r\n\t=+-|*%@#$^&";
			this._quotation		= this.str2hashtable("\",'");
			this._lineComment	= "//";
			this._escape		= "\\";
			this._commentOn		= "/*";
			this._commentOff	= "*/";
			this._ignore		= "<!--";
			break;
		case "py":
			this._caseSensitive	= false;
			this._keywords		= this.str2hashtable("and,assert,break,class,continue,def,del,elif,else,except,exec,finally,for,from,global,if,import,in,is,lambda,not,or,pass,print,raise,return,try,yield,while,None,True,False,self,cls,class");
			this._commonObjects = this.str2hashtable("");
			this._tags			= this.str2hashtable("",false);
			this._wordDelimiters= " ,.?!;:\\/<>(){}[]\"'\r\n\t=+-|*%@#$^&";
			this._quotation		= this.str2hashtable("\",'");
			this._lineComment	= "//";
			this._escape		= "\\";
			this._commentOn		= "/*";
			this._commentOff	= "*/";
			this._ignore		= "<!--";
			break;
		case "asm":
			this._caseSensitive	= false;
			this._keywords		= this.str2hashtable("FRSTOR,LOOPDNE,LOOPD,PREFIX,REPNE,AAA,AAD,AAM,AAS,ADC,ADD,AND,ARPL,BOUND,CBW,CLC,CLD,CLI,CLTS,CMC,CMP,CMPSB,CMPSW,CWD,DAA,DAS,DEC,DIV,ENTER,ESC,HLT,IDIV,IMUL,IN,INC,INSB,INSW,INT,INTO,IRET,LAHF,LAR,LDS,LEA,LEAVE,LES,LGDT,LIDT,LLDT,LMSW,LOCK,LODSB,LODSW,LOOP,LOOPNZ,LOOPZ,LSL,LTR,MOV,MOVSB,MOVSW,MUL,NEG,NOP,NOT,OR,OUT,OUTSB,OUTSW,POP,POPA,POPF,PUSH,PUSHA,PUSHF,RCL,RCR,REP,REPNZ,REPZ,RET,RETN,REFT,ROL,ROR,SAHF,SAR,SAL,SBB,SCASB,SCASW,SGDT,SHL,SHR,SLDT,SMSW,STC,STD,STI,STOSB,STOSW,STR,SUB,TEST,WAIT,VERR,VERW,XCHG,XLAT,XOR,BSF,BSR,BT,BTC,BTR,BTS,CDQ,CWDE,IRETD,LFS,LGS,LSS,MOVSX,MOVZX,POPAD,POPFD,PUSHAD,PUSHFD,SETA,SETB,SETBE,SETE,SETG,SETL,SETLE,SETNB,SETNE,SETNL,SETNO,SETNP,SETNS,SETO,SETP,SETS,SHLD,SHRD,CMPSD,STOSD,LODSD,MOVSD,SCASD,INSD,OUTSD,JECXZ,BSWAP,CMPXCHG,INVD,INVLPG,WBINVD,XADD,FABS,FADD,FADDP,FBLD,FBSTP,FCHS,FCLEX,FCOM,FCOMP,FCOMPP,FDECSTP,FDISI,FDIV,FDIVP,FDIVR,FDIVRP,FENI,FFREE,FIADD,FIACOM,FIACOMP,FIDIV,FIDIVR,FILD,FIMUL,FINCSTP,FINIT,FIST,FISTP,FISUB,FISUBR,FLD,FLDCWR,FLDENV,FLDLG2,FLDLN2,FLDL2E,FLDL2T,FLDPI,FLDZ,FLD1,FLDCW,FMUL,FMULP,FNOP,FNSTS,FPATAN,FPREM,FPTAN,FRNDINT,FSAVENT,FSCALE,FSETPM,FSQRT,FST,FSTCW,FSTENV,FSTP,FSTSW,FSUB,FSUBP,FSUBR,FSUBRP,FTST,FWAIT,FXAM,FXCH,FXTRACT,FYL2X,FYL2XPI,F2XM1,FCOS,FSIN,FPREM1,FSINCOS,FUCOM,FUCOMP,FUCOMPP,CALL");
			this._commonObjects = this.str2hashtable("EAX,AH,AL,EBX,BX,BH,BL,ECX,CX,CH,CL,EDX,DX,DH,DL,ESI,SI,EDI,DI,ESP,SP,EBP,BP,EFLAGS,FLAGS,CS,DS,ES,SS,FS,GS,ST,CR,DR,TR,GDTR,LDTR,IDTR,WORD,BYTE,DWORD,QWORD,FWORD,TBYTE,PTR,SHORT,NEAR,JA,JAE,JB,JBE,JCXZ,JC,JNC,JE,JNGE,JNG,JNL,JG,JGE,JL,JLE,JMP,JNB,JNBE,JNE,JNLE,JNO,JNP,JNZ,JPO,JZ,JO,JP,JS,JNS");	
			this._tags			= this.str2hashtable("",false);
			this._wordDelimiters= " \n\r\t,/?:;\"'{[}]~`%^()*-+=!";
			this._quotation		= this.str2hashtable("\",'");
			this._lineComment	= ";";
			this._escape		= "\\";
			this._commentOn		= "";
			this._commentOff	= "";
			this._ignore		= "";
			break;
		case "c":
		case "cpp":
		case "cxx":
		case "cc":
		default:
			this._caseSensitive	= true;
			this._keywords		= this.str2hashtable("upper_bound,lower_bound,binary_search,equal_range,make_heap,sort_heap,popheap,push_heap,sort,reverse,copy,null,NULL,comment,lib,in,out,break,case,catch,class,const,__finally,__exception,__try,const_cast,continue,private,public,protected,__declspec,default,delete,deprecated,dllexport,dllimport,do,dynamic_cast,else,enum,explicit,extern,if,for,friend,goto,inline,mutable,naked,new,noinline,noreturn,nothrow,register,reinterpret_cast,return,selectany,sizeof,static,static_cast,struct,switch,template,this,thread,throw,true,false,TRUE,FALSE,try,typedef,typeid,typename,union,uuid,virtual,void,volatile,istream,ostring,ifstream,ofstream,iteratorstruct,using,namespace,whcar_t,_asm,__asm,while,#include,#if,#elif,#else,#ifdef,#ifndef,#define,#undef,#endif,#pragma,#line");
			this._commonObjects = this.str2hashtable("ATOM,BOOL,BOOLEAN,BYTE,CHAR,COLORREF,DWORD,DWORDLONG,DWORD_PTR,DWORD32,DWORD64,FLOAT,HACCEL,HALF_PTR,HANDLE,HBITMAP,HBRUSH,HCOLORSPACE,HCONV,HCONVLIST,HCURSOR,HDC,HDDEDATA,HDESK,HDROP,HDWP,HENHMETAFILE,HFILE,HFONT,HGDIOBJ,HGLOBAL,HHOOK,HICON,HINSTANCE,HKEY,HKL,HLOCAL,HMENU,HMETAFILE,HMODULE,HMONITOR,HPALETTE,HPEN,HRESULT,HRGN,HRSRC,HSZ,HWINSTA,HWND,INT,INT_PTR,INT32,INT64,LANGID,LCID,LCTYPE,LGRPID,LONG,LONGLONG,LONG_PTR,LONG32,LONG64,LPARAM,LPBOOL,LPBYTE,LPCOLORREF,LPCSTR,LPCTSTR,LPCVOID,LPCWSTR,LPDWORD,LPHANDLE,LPINT,LPLONG,LPSTR,LPTSTR,LPVOID,LPWORD,LPWSTR,LRESULT,PBOOL,PBOOLEAN,PBYTE,PCHAR,PCSTR,PCTSTR,PCWSTR,PDWORDLONG,PDWORD_PTR,PDWORD32,PDWORD64,PFLOAT,PHALF_PTR,PHANDLE,PHKEY,PINT,PINT_PTR,PINT32,PINT64,PLCID,PLONG,PLONGLONG,PLONG_PTR,PLONG32,PLONG64,POINTER_32,POINTER_64,PSHORT,PSIZE_T,PSSIZE_T,PSTR,PTBYTE,PTCHAR,PTSTR,PUCHAR,PUHALF_PTR,PUINT,PUINT_PTR,PUINT32,PUINT64,PULONG,PULONGLONG,PULONG_PTR,PULONG32,PULONG64,PUSHORT,PVOID,PWCHAR,PWORD,PWSTR,SC_HANDLE,SC_LOCK,SERVICE_STATUS_HANDLE,SHORT,SIZE_T,SSIZE_T,TBYTE,TCHAR,UCHAR,UHALF_PTR,UINT,UINT_PTR,UINT32,UINT64,ULONG,ULONGLONG,ULONG_PTR,ULONG32,ULONG64,USHORT,USN,VOID,WCHAR,WORD,WPARAM,STRING,atom,bool,boolean,byte,char,colorref,dword,dwordlong,dword_ptr,dword32,dword64,float,haccel,half_ptr,handle,hbitmap,hbrush,hcolorspace,hconv,hconvlist,hcursor,hdc,hddedata,hdesk,hdrop,hdwp,henhmetafile,hfile,hfont,hgdiobj,hglobal,hhook,hicon,hinstance,hkey,hkl,hlocal,hmenu,hmetafile,hmodule,hmonitor,hpalette,hpen,hresult,hrgn,hrsrc,hsz,hwinsta,hwnd,int,int_ptr,int32,int64,langid,lcid,lctype,lgrpid,long,longlong,long_ptr,long32,long64,lparam,lpbool,lpbyte,lpcolorref,lpcstr,lpctstr,lpcvoid,lpcwstr,lpdword,lphandle,lpint,lplong,lpstr,lptstr,lpvoid,lpword,lpwstr,lresult,pbool,pboolean,pbyte,pchar,pcstr,pctstr,pcwstr,pdwordlong,pdword_ptr,pdword32,pdword64,pfloat,phalf_ptr,phandle,phkey,pint,pint_ptr,pint32,pint64,plcid,plong,plonglong,plong_ptr,plong32,plong64,pointer_32,pointer_64,pshort,psize_t,pssize_t,pstr,ptbyte,ptchar,ptstr,puchar,puhalf_ptr,puint,puint_ptr,puint32,puint64,pulong,pulonglong,pulong_ptr,pulong32,pulong64,pushort,pvoid,pwchar,pword,pwstr,sc_handle,sc_lock,service_status_handle,short,size_t,ssize_t,tbyte,tchar,uchar,uhalf_ptr,uint,uint_ptr,uint32,uint64,ulong,ulonglong,ulong_ptr,ulong32,ulong64,ushort,usn,void,wchar,word,wparam,string,NTSTATUS,PIRP,PDEVICE_OBJECT,PDRIVER_OBJECT,PUNICODE_STRING,UNICODE_STRING,char,bool,short,int,__int32,__int64,__int8,__int16,long,float,double,__wchar_t,clock_t,_complex,_dev_t,_diskfree_t,div_t,ldiv_t,_exception,_EXCEPTION_POINTERS,FILE,_finddata_t,_finddatai64_t,_wfinddata_t,_wfinddatai64_t,__finddata64_t,__wfinddata64_t,_FPIEEE_RECORD,fpos_t,_HEAPINFO,_HFILE,lconv,intptr_t,jmp_buf,mbstate_t,_off_t,_onexit_t,_PNH,ptrdiff_t,_purecall,_handler,sig_atomic_t,size_t,_stat,__stat64,_stati64,terminate_function,time_t,__time64_t,_timeb,__timeb64,tm,uintptr_t,_utimbuf,va_list,wchar_t,wctrans_t,wctype_t,wint_t,signed,main,printf,vector,VECTOR,queue,QUEUE,priority_queue,PRIORITY_QUEUE,deque,DEQUE,stack,STACK,pair,PAIR");
			this._tags			= this.str2hashtable("",false);
			this._wordDelimiters= " ,.?!;:\/<>(){}[]\"'\r\n\t\\=+-|*%@$^&";
			this._quotation		= this.str2hashtable("\",'");
			this._lineComment	= "//";
			this._escape		= "\\";
			this._commentOn		= "/*";
			this._commentOff	= "*/";
			this._ignore		= "";
			break;
	}
	
	this._operators =",.?!;:\\/<>(){}[]=+-|*%@#$^&";

	this.highlight	= function(){
		var codeArr = new Array();
		var word_index = 0;
		var htmlTxt = new Array();

		//得到分割字符数组(分词)
		for (var i=0; i<this._codetxt.length; i++) {
			if (this._wordDelimiters.indexOf(this._codetxt.charAt(i)) == -1) {
				if (codeArr[word_index] == null || typeof(codeArr[word_index]) == 'undefined') {
					codeArr[word_index] = "";
				}
				codeArr[word_index] += this._codetxt.charAt(i);
			} else {
				if (typeof(codeArr[word_index]) != 'undefined' && codeArr[word_index].length > 0)
					word_index++;
				codeArr[word_index++] = this._codetxt.charAt(i);
			}
		}

		var quote_opened				= false;	//引用标记
		var slash_star_comment_opened	= false;	//多行注释标记
		var slash_slash_comment_opened	= false;	//单行注释标记
		var line_num					= 1;		//行号
		var quote_char					= "";		//引用标记类型
		var tag_opened					= false;	//标记开始
		
		htmlTxt[htmlTxt.length] = ("<pre class='code'><ol><li>");
		
		//按分割字，分块显示
		for (var i=0;i <=word_index;i++)
		{		
			//处理空行（由于转义带来）
			if(typeof(codeArr[i])=="undefined"||codeArr[i].length==0){
				continue;
			}//处理空格
			if (codeArr[i] == " "){
				htmlTxt[htmlTxt.length] = ("&nbsp;");
			//处理关键字
			} else if (!slash_slash_comment_opened && !slash_star_comment_opened && !quote_opened && this.isKeyword(codeArr[i])){
				if(codeArr[i-3]=="#include" && codeArr[i-1]=="<" && codeArr[i+1]==">"){htmlTxt[htmlTxt.length] = codeArr[i];}
				else{htmlTxt[htmlTxt.length] = ("<em>" + codeArr[i] + "</em>");}
			//处理普通对象
			} else if (!slash_slash_comment_opened && !slash_star_comment_opened && !quote_opened && this.isCommonObject(codeArr[i])){
				if(codeArr[i-3]=="#include" && codeArr[i-1]=="<" && codeArr[i+1]==">"){htmlTxt[htmlTxt.length] = codeArr[i];}
				else{htmlTxt[htmlTxt.length] = ("<dfn>" + codeArr[i] + "</dfn>");}		
			//处理标记
			} else if (!slash_slash_comment_opened && !slash_star_comment_opened && !quote_opened && tag_opened && this.isTag(codeArr[i])){
				htmlTxt[htmlTxt.length] = ("<tt>" + codeArr[i] + "</tt>");
			//处理换行
			} else if (codeArr[i] == "\n"){			
				if (slash_slash_comment_opened){
					htmlTxt[htmlTxt.length] = ("</cite></li><li>");
					slash_slash_comment_opened = false;
				} else if(slash_star_comment_opened){
					htmlTxt[htmlTxt.length] = ("</cite></li><li><cite>");			
				} else {
					htmlTxt[htmlTxt.length] = ("</li><li>");}
				line_num++;
			//处理双引号（引号前不能为转义字符）
			} else if (this._quotation.contains(codeArr[i])&&!slash_star_comment_opened&&!slash_slash_comment_opened){
				if (quote_opened){
					//是相应的引号
					if (quote_char==codeArr[i]){
						if(tag_opened){
							htmlTxt[htmlTxt.length] = (codeArr[i]+"</samp><samp>");
						} else {
							htmlTxt[htmlTxt.length] = (codeArr[i]+"</samp>");
						}
						quote_opened= false;
						quote_char	= "";
					} else {
						htmlTxt[htmlTxt.length] = codeArr[i].replace(/\</g,"&lt;");
					}
				} else {
					if (tag_opened){
						htmlTxt[htmlTxt.length] =  ("</tt><samp>" + codeArr[i]);
					} else {
						htmlTxt[htmlTxt.length] =  ("<samp>" + codeArr[i]);
					}
					quote_opened	= true;
					quote_char		= codeArr[i];
				}
			//处理转义字符
			} else if(codeArr[i] == this._escape){
				htmlTxt[htmlTxt.length] = (codeArr[i]);
				if (i<word_index){
					if (codeArr[i+1].charCodeAt(0)>=32&&codeArr[i+1].charCodeAt(0)<=127){
						htmlTxt[htmlTxt.length] = codeArr[i+1].substr(0,1);
						codeArr[i+1] = codeArr[i+1].substr(1);
					}
				}
			//处理Tab
			} else if (codeArr[i] == "\t") {
				htmlTxt[htmlTxt.length] = ("&nbsp;&nbsp;&nbsp;&nbsp;");
			//处理多行注释的开始
			} else if (this.isStartWith(this._commentOn,codeArr,i)&&!slash_slash_comment_opened && !slash_star_comment_opened &&!quote_opened){
				slash_star_comment_opened = true;
				htmlTxt[htmlTxt.length] =  ("<cite>" + this._commentOn.replace(/\</g,"&lt"));
				i = i + this._commentOn.length-1;
			//处理单行注释
			} else if (this.isStartWith(this._lineComment,codeArr,i)&&!slash_slash_comment_opened && !slash_star_comment_opened&&!quote_opened){
				slash_slash_comment_opened = true;
				htmlTxt[htmlTxt.length] =  ("<cite>" + this._lineComment);
				i = i + this._lineComment.length-1;
			//处理忽略词
			} else if (this.isStartWith(this._ignore,codeArr,i)&&!slash_slash_comment_opened && !slash_star_comment_opened&&!quote_opened){
				slash_slash_comment_opened = true;
				htmlTxt[htmlTxt.length] =  ("<cite>" + this._ignore.replace(/\</g,"&lt"));
				i = i + this._ignore.length-1;
			//处理多行注释结束
			} else if (this.isStartWith(this._commentOff,codeArr,i)&&!quote_opened&&!slash_slash_comment_opened){
				if (slash_star_comment_opened) {
					slash_star_comment_opened = false;
					htmlTxt[htmlTxt.length] =  (this._commentOff +"</samp>");
					i = i + this._commentOff.length-1;
				}
			//处理左标记
			} else if (this._dealTag&&!slash_slash_comment_opened && !slash_star_comment_opened&&!quote_opened&&codeArr[i] == "<") {
				htmlTxt[htmlTxt.length] = "&lt;<tt>";
				tag_opened	= true;
			//处理右标记
			} else if (this._dealTag&&tag_opened&&codeArr[i] == ">") {
				htmlTxt[htmlTxt.length] = "</tt>&gt;";
				tag_opened	= false;
			//处理数字
			} else if (!isNaN(codeArr[i])&&!slash_slash_comment_opened&&!slash_star_comment_opened && !quote_opened){			
				htmlTxt[htmlTxt.length] = ("<s>" + codeArr[i] + "</s>");
			//处理运算符
			} else if (!slash_slash_comment_opened && !slash_star_comment_opened && !quote_opened && (this._operators.indexOf(codeArr[i])>0)){
				htmlTxt[htmlTxt.length] = ("<b>" + codeArr[i] + "</b>");
			//处理HTML转义符号
			} else if (codeArr[i] == "&") {
				htmlTxt[htmlTxt.length] = "&amp;";
			} else {
				htmlTxt[htmlTxt.length] = codeArr[i].replace(/\</g,"&lt;");
			}
		}
		
		if(slash_slash_comment_opened) htmlTxt[htmlTxt.length] = ("</cite>");

		htmlTxt[htmlTxt.length] = ("</li></ol></pre>");
		return htmlTxt.join("");
	}

	this.isStartWith = function(str,code,index){
		if(typeof(str)!="undefined"&&str.length>0){
			for(var i=0;i<str.length;i++){
				if(this._caseSensitive){
					if(str.charAt(i)!=code[index+i]||(index+i>=code.length)){
						return false;
					}
				} else {
					if(str.charAt(i).toLowerCase()!=code[index+i].toLowerCase()||(index+i>=code.length)){
						return false;
					}
				}
			}
			return true;
		} else {
			return false;
		}
	}

	this.isKeyword = function(val){
		return this._keywords.contains(this._caseSensitive?val:val.toLowerCase());
	}

	this.isCommonObject = function(val){
		return this._commonObjects.contains(this._caseSensitive?val:val.toLowerCase());
	}

	this.isTag = function(val){
		return this._tags.contains(val.toLowerCase());
	}
}
            ]]>
          </xsl:text>
        </script>
      </head>
      <body>
        <h2>测试报告</h2>
        <ul>
          <li>
            序号: <xsl:value-of select="Id" />
          </li>
          <li>
            文件名: <xsl:value-of select="FileName" />
          </li>
          <li>
            日期: <xsl:value-of select="Date" />
          </li>
          <li>
            状态: <xsl:value-of select="Flag" />
          </li>
          <li>
            分数: <xsl:value-of select="Score" />
          </li>
          <li>
            时间: <xsl:value-of select="TimeUsage" />
          </li>
          <li>
            内存: <xsl:value-of select="MemoryUsage" />
          </li>
          <li>
            警告: <xsl:value-of select="Warning" />
          </li>
        </ul>
        <table border="1" cellspacing="0">
          <tr>
            <th>序号</th>
            <th>状态</th>
            <th>分数</th>
            <th>时间</th>
            <th>内存</th>
            <th>警告</th>
          </tr>
          <xsl:for-each select="TestEntry">
            <tr>
              <td>
                <xsl:value-of select="Index" />
              </td>
              <td>
                <xsl:value-of select="Flag" />
              </td>
              <td>
                <xsl:value-of select="Score" />
              </td>
              <td>
                <xsl:value-of select="TimeUsage" />
              </td>
              <td>
                <xsl:value-of select="MemoryUsage" />
              </td>
              <td>
                <xsl:value-of select="Warning" />
              </td>
            </tr>
          </xsl:for-each>
        </table>
        <h3>源代码:</h3>
        <span id="codeFrame"></span>
        <textarea id="code" style="display:none">
          <xsl:value-of select="SourceCode" />
        </textarea>
        <script language="JavaScript" type="text/javascript">
          OnColor("<xsl:value-of select="Extension" />");
        </script>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
