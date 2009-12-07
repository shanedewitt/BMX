KIDS Distribution saved on Dec 07, 2009@11:11:21
Modified XWB Routine to correct $$OS bug and support BMX.net
**KIDS**:XWB*1.1*113102^

**INSTALL NAME**
XWB*1.1*113102
"BLD",7415,0)
XWB*1.1*113102^RPC BROKER^0^3091207^n
"BLD",7415,1,0)
^9.61A^9^9^3091207^^
"BLD",7415,1,1,0)
This patch adds support to XWB of routing BMX Broker messages to the 
"BLD",7415,1,2,0)
BMXMON routine. As such, it provides a uniform entry point for all broker 
"BLD",7415,1,3,0)
messaging.
"BLD",7415,1,4,0)
 
"BLD",7415,1,5,0)
Produced on July 22 2009 by Sam Habiel for WorldVista.
"BLD",7415,1,6,0)
 
"BLD",7415,1,7,0)
Licensed under WorldVista global license, currently GPL 2.
"BLD",7415,1,8,0)
 
"BLD",7415,1,9,0)
**updated on Aug 29th to handle IPv6 addresses for GT.M**
"BLD",7415,4,0)
^9.64PA^^
"BLD",7415,6.3)
6
"BLD",7415,"KRN",0)
^9.67PA^8989.52^19
"BLD",7415,"KRN",.4,0)
.4
"BLD",7415,"KRN",.401,0)
.401
"BLD",7415,"KRN",.402,0)
.402
"BLD",7415,"KRN",.403,0)
.403
"BLD",7415,"KRN",.5,0)
.5
"BLD",7415,"KRN",.84,0)
.84
"BLD",7415,"KRN",3.6,0)
3.6
"BLD",7415,"KRN",3.8,0)
3.8
"BLD",7415,"KRN",9.2,0)
9.2
"BLD",7415,"KRN",9.8,0)
9.8
"BLD",7415,"KRN",9.8,"NM",0)
^9.68A^1^1
"BLD",7415,"KRN",9.8,"NM",1,0)
XWBTCPM^^0^B56820596
"BLD",7415,"KRN",9.8,"NM","B","XWBTCPM",1)

"BLD",7415,"KRN",19,0)
19
"BLD",7415,"KRN",19.1,0)
19.1
"BLD",7415,"KRN",101,0)
101
"BLD",7415,"KRN",409.61,0)
409.61
"BLD",7415,"KRN",771,0)
771
"BLD",7415,"KRN",870,0)
870
"BLD",7415,"KRN",8989.51,0)
8989.51
"BLD",7415,"KRN",8989.52,0)
8989.52
"BLD",7415,"KRN",8994,0)
8994
"BLD",7415,"KRN","B",.4,.4)

"BLD",7415,"KRN","B",.401,.401)

"BLD",7415,"KRN","B",.402,.402)

"BLD",7415,"KRN","B",.403,.403)

"BLD",7415,"KRN","B",.5,.5)

"BLD",7415,"KRN","B",.84,.84)

"BLD",7415,"KRN","B",3.6,3.6)

"BLD",7415,"KRN","B",3.8,3.8)

"BLD",7415,"KRN","B",9.2,9.2)

"BLD",7415,"KRN","B",9.8,9.8)

"BLD",7415,"KRN","B",19,19)

"BLD",7415,"KRN","B",19.1,19.1)

"BLD",7415,"KRN","B",101,101)

"BLD",7415,"KRN","B",409.61,409.61)

"BLD",7415,"KRN","B",771,771)

"BLD",7415,"KRN","B",870,870)

"BLD",7415,"KRN","B",8989.51,8989.51)

"BLD",7415,"KRN","B",8989.52,8989.52)

"BLD",7415,"KRN","B",8994,8994)

"BLD",7415,"QDEF")
^^^^NO^^^^NO^^NO
"BLD",7415,"QUES",0)
^9.62^^
"MBREQ")
0
"PKG",70,-1)
1^1
"PKG",70,0)
RPC BROKER^XWB^Remote Procedure Call Broker
"PKG",70,20,0)
^9.402P^^
"PKG",70,22,0)
^9.49I^1^1
"PKG",70,22,1,0)
1.1^3020529^2971118^1
"PKG",70,22,1,"PAH",1,0)
113102^3091207
"PKG",70,22,1,"PAH",1,1,0)
^^9^9^3091207
"PKG",70,22,1,"PAH",1,1,1,0)
This patch adds support to XWB of routing BMX Broker messages to the 
"PKG",70,22,1,"PAH",1,1,2,0)
BMXMON routine. As such, it provides a uniform entry point for all broker 
"PKG",70,22,1,"PAH",1,1,3,0)
messaging.
"PKG",70,22,1,"PAH",1,1,4,0)
 
"PKG",70,22,1,"PAH",1,1,5,0)
Produced on July 22 2009 by Sam Habiel for WorldVista.
"PKG",70,22,1,"PAH",1,1,6,0)
 
"PKG",70,22,1,"PAH",1,1,7,0)
Licensed under WorldVista global license, currently GPL 2.
"PKG",70,22,1,"PAH",1,1,8,0)
 
"PKG",70,22,1,"PAH",1,1,9,0)
**updated on Aug 29th to handle IPv6 addresses for GT.M**
"QUES","XPF1",0)
Y
"QUES","XPF1","??")
^D REP^XPDH
"QUES","XPF1","A")
Shall I write over your |FLAG| File
"QUES","XPF1","B")
YES
"QUES","XPF1","M")
D XPF1^XPDIQ
"QUES","XPF2",0)
Y
"QUES","XPF2","??")
^D DTA^XPDH
"QUES","XPF2","A")
Want my data |FLAG| yours
"QUES","XPF2","B")
YES
"QUES","XPF2","M")
D XPF2^XPDIQ
"QUES","XPI1",0)
YO
"QUES","XPI1","??")
^D INHIBIT^XPDH
"QUES","XPI1","A")
Want KIDS to INHIBIT LOGONs during the install
"QUES","XPI1","B")
NO
"QUES","XPI1","M")
D XPI1^XPDIQ
"QUES","XPM1",0)
PO^VA(200,:EM
"QUES","XPM1","??")
^D MG^XPDH
"QUES","XPM1","A")
Enter the Coordinator for Mail Group '|FLAG|'
"QUES","XPM1","B")

"QUES","XPM1","M")
D XPM1^XPDIQ
"QUES","XPO1",0)
Y
"QUES","XPO1","??")
^D MENU^XPDH
"QUES","XPO1","A")
Want KIDS to Rebuild Menu Trees Upon Completion of Install
"QUES","XPO1","B")
NO
"QUES","XPO1","M")
D XPO1^XPDIQ
"QUES","XPZ1",0)
Y
"QUES","XPZ1","??")
^D OPT^XPDH
"QUES","XPZ1","A")
Want to DISABLE Scheduled Options, Menu Options, and Protocols
"QUES","XPZ1","B")
NO
"QUES","XPZ1","M")
D XPZ1^XPDIQ
"QUES","XPZ2",0)
Y
"QUES","XPZ2","??")
^D RTN^XPDH
"QUES","XPZ2","A")
Want to MOVE routines to other CPUs
"QUES","XPZ2","B")
NO
"QUES","XPZ2","M")
D XPZ2^XPDIQ
"RTN")
1
"RTN","XWBTCPM")
0^1^B56820596
"RTN","XWBTCPM",1,0)
XWBTCPM ;ISF/RWF - BROKER TCP/IP PROCESS HANDLER ; 12/7/09 10:30am
"RTN","XWBTCPM",2,0)
 ;;1.1;RPC BROKER;**35,43,49**;Mar 28, 1997;Build 6
"RTN","XWBTCPM",3,0)
 ;Local patch 113102 by WV/SMH for BMX.net support
"RTN","XWBTCPM",4,0)
 ;Based on: XWBTCPC & XWBTCPL, Modified by ISF/RWF
"RTN","XWBTCPM",5,0)
 ;Changed to be started by UCX or %ZISTCPS
"RTN","XWBTCPM",6,0)
 ;
"RTN","XWBTCPM",7,0)
DSM ;DSM called from ucx, % passed in with device.
"RTN","XWBTCPM",8,0)
 D ESET
"RTN","XWBTCPM",9,0)
 ;Open the device
"RTN","XWBTCPM",10,0)
 S XWBTDEV=% X "O XWBTDEV:(TCPDEV):60" ;Special UCX/DSM open
"RTN","XWBTCPM",11,0)
 ;Go find the connection type
"RTN","XWBTCPM",12,0)
 U XWBTDEV
"RTN","XWBTCPM",13,0)
 G CONNTYPE
"RTN","XWBTCPM",14,0)
 ;
"RTN","XWBTCPM",15,0)
CACHEVMS ;Cache'/VMS tcpip entry point, called from XWBTCP_START.COM file
"RTN","XWBTCPM",16,0)
 D ESET
"RTN","XWBTCPM",17,0)
 S XWBTDEV="SYS$NET"
"RTN","XWBTCPM",18,0)
 ; **Cache'/VMS specific code**
"RTN","XWBTCPM",19,0)
 O XWBTDEV::5
"RTN","XWBTCPM",20,0)
 X "U XWBTDEV:(::""-M"")" ;Packet mode like DSM
"RTN","XWBTCPM",21,0)
 G CONNTYPE
"RTN","XWBTCPM",22,0)
 ;
"RTN","XWBTCPM",23,0)
NT ;entry from ZISTCPS
"RTN","XWBTCPM",24,0)
 ;JOB LISTEN^%ZISTCPS("port","NT^XWBTCPM","stop code")
"RTN","XWBTCPM",25,0)
 D ESET
"RTN","XWBTCPM",26,0)
 S XWBTDEV=IO
"RTN","XWBTCPM",27,0)
 G CONNTYPE
"RTN","XWBTCPM",28,0)
 ;
"RTN","XWBTCPM",29,0)
GTMUCX(%) ;From ucx ZFOO
"RTN","XWBTCPM",30,0)
 ;If called from LISTEN^%ZISTCP(PORT,"GTM^XWBTCPM") S XWBTDEV=IO
"RTN","XWBTCPM",31,0)
 D ESET
"RTN","XWBTCPM",32,0)
 ;GTM specific code
"RTN","XWBTCPM",33,0)
 S @("$ZINTERRUPT=""I $$JOBEXAM^ZU($ZPOSITION)""")
"RTN","XWBTCPM",34,0)
 S XWBTDEV=% X "O %:(RECORDSIZE=512)"
"RTN","XWBTCPM",35,0)
 G CONNTYPE
"RTN","XWBTCPM",36,0)
 ;
"RTN","XWBTCPM",37,0)
GTMLNX ;From Linux xinetd script
"RTN","XWBTCPM",38,0)
 D ESET
"RTN","XWBTCPM",39,0)
 ;GTM specific code
"RTN","XWBTCPM",40,0)
 S @("$ZINTERRUPT=""I $$JOBEXAM^ZU($ZPOSITION)""")
"RTN","XWBTCPM",41,0)
 S XWBTDEV=$P X "U XWBTDEV:(nowrap:nodelimiter:ioerror=""TRAP"")"
"RTN","XWBTCPM",42,0)
 S %="",@("%=$ZTRNLNM(""REMOTE_HOST"")") S:$L(%) IO("GTM-IP")=%
"RTN","XWBTCPM",43,0)
 I %["::ffff:" S IO("GTM-IP")=$P(%,"::ffff:",2) ; fake ipv6 support
"RTN","XWBTCPM",44,0)
 G CONNTYPE
"RTN","XWBTCPM",45,0)
 ;
"RTN","XWBTCPM",46,0)
ESET ;Set inital error trap
"RTN","XWBTCPM",47,0)
 S U="^",$ETRAP="D ^%ZTER H" ;Set up the error trap
"RTN","XWBTCPM",48,0)
 S X="",@^%ZOSF("TRAP") ;Clear old trap
"RTN","XWBTCPM",49,0)
 Q
"RTN","XWBTCPM",50,0)
 ;Find the type of connection and jump to the processing routine.
"RTN","XWBTCPM",51,0)
CONNTYPE ;
"RTN","XWBTCPM",52,0)
 N XWBDEBUG,XWBAPVER,XWBCLMAN,XWBENVL,XWBLOG,XWBOS,XWBPTYPE
"RTN","XWBTCPM",53,0)
 N XWBTBUF,XWBTIP,XWBTSKT,XWBVER,XWBWRAP,XWBSHARE,XWBT
"RTN","XWBTCPM",54,0)
 N SOCK,TYPE
"RTN","XWBTCPM",55,0)
 D INIT
"RTN","XWBTCPM",56,0)
 S XWB=$$BREAD^XWBRW(5,XWBTIME)
"RTN","XWBTCPM",57,0)
 D LOG("MSG format is "_XWB_" type "_$S(XWB="[XWB]":"NEW",XWB="{XWB}":"OLD",XWB="<?xml":"M2M",XWB="{BMX}":"BMX",1:"Unk"))
"RTN","XWBTCPM",58,0)
 I XWB["[XWB]" G NEW
"RTN","XWBTCPM",59,0)
 I XWB["{XWB}" G OLD^XWBTCPM1
"RTN","XWBTCPM",60,0)
 I XWB["<?xml" G M2M
"RTN","XWBTCPM",61,0)
 I XWB["{BMX}" G GTMLNX^BMXMON
"RTN","XWBTCPM",62,0)
 I $L($T(OTH^XWBTCPM2)) D OTH^XWBTCPM2 ;See if a special code.
"RTN","XWBTCPM",63,0)
 D LOG("Prefix not known: "_XWB)
"RTN","XWBTCPM",64,0)
 Q
"RTN","XWBTCPM",65,0)
 ;
"RTN","XWBTCPM",66,0)
NEWJOB() ;Check if OK to start a new job, Return 1 if OK, 0 if not OK.
"RTN","XWBTCPM",67,0)
 N X,Y,J,XWBVOL
"RTN","XWBTCPM",68,0)
 D GETENV^%ZOSV S XWBVOL=$P(Y,"^",2)
"RTN","XWBTCPM",69,0)
 S X=$O(^XTV(8989.3,1,4,"B",XWBVOL,0)),J=$S(X>0:^XTV(8989.3,1,4,X,0),1:"ROU^y^1")
"RTN","XWBTCPM",70,0)
 I $G(^%ZIS(14.5,"LOGON",XWBVOL)) Q 0 ;Check INHIBIT LOGONS?
"RTN","XWBTCPM",71,0)
 I $D(^%ZOSF("ACTJ")) X ^("ACTJ") I $P(J,U,3),($P(J,U,3)'>Y) Q 0
"RTN","XWBTCPM",72,0)
 Q 1
"RTN","XWBTCPM",73,0)
 ;
"RTN","XWBTCPM",74,0)
M2M ;M2M Broker
"RTN","XWBTCPM",75,0)
 S XWBRBUF=XWB_XWBRBUF,(IO,IO(0))=XWBTDEV G SPAWN^XWBVLL
"RTN","XWBTCPM",76,0)
 Q
"RTN","XWBTCPM",77,0)
 ;
"RTN","XWBTCPM",78,0)
NEW ;New broker
"RTN","XWBTCPM",79,0)
 S U="^",DUZ=0,DUZ(0)="",XWBVER=1.108
"RTN","XWBTCPM",80,0)
 D SETTIME(1) ;Setup for sign-on timeout
"RTN","XWBTCPM",81,0)
 U XWBTDEV D
"RTN","XWBTCPM",82,0)
 . N XWB,ERR,NATIP,I
"RTN","XWBTCPM",83,0)
 . S ERR=$$PRSP^XWBPRS
"RTN","XWBTCPM",84,0)
 . S ERR=$$PRSM^XWBPRS
"RTN","XWBTCPM",85,0)
 . S MSG=$G(XWB(4,"CMD")) ;Build connect msg.
"RTN","XWBTCPM",86,0)
 . S I="" F  S I=$O(XWB(5,"P",I)) Q:I=""  S MSG=MSG_U_XWB(5,"P",I)
"RTN","XWBTCPM",87,0)
 . ;Get the peer and save that IP.
"RTN","XWBTCPM",88,0)
 . S NATIP=$$GETPEER^%ZOSV S:'$L(NATIP) NATIP=$P(MSG,"^",2)
"RTN","XWBTCPM",89,0)
 . I NATIP'=$P(MSG,"^",2) S $P(MSG,"^",2)=NATIP
"RTN","XWBTCPM",90,0)
 . Q
"RTN","XWBTCPM",91,0)
 S X=$$NEWJOB() D:'X LOG("No New Connects")
"RTN","XWBTCPM",92,0)
 I ($P(MSG,U)'="TCPConnect")!('X) D QSND^XWBRW("reject"),LOG("reject: "_MSG) Q
"RTN","XWBTCPM",93,0)
 D QSND^XWBRW("accept"),LOG("accept") ;Ack
"RTN","XWBTCPM",94,0)
 S IO("IP")=$P(MSG,U,2),XWBTSKT=$P(MSG,U,3),XWBCLMAN=$P(MSG,U,4)
"RTN","XWBTCPM",95,0)
 S XWBTIP=$G(IO("IP"))
"RTN","XWBTCPM",96,0)
 ;start RUM for Broker Handler XWB*1.1*5
"RTN","XWBTCPM",97,0)
 D LOGRSRC^%ZOSV("$BROKER HANDLER$",2,1)
"RTN","XWBTCPM",98,0)
 ;GTM
"RTN","XWBTCPM",99,0)
 I $G(XWBT("PCNT")) D
"RTN","XWBTCPM",100,0)
 . S X=$NA(^XUTL("XUSYS",$J,1)) L +@X:0
"RTN","XWBTCPM",101,0)
 . D COUNT^XUSCNT(1),SETLOCK^XUSCNT(X)
"RTN","XWBTCPM",102,0)
 ;We don't use a callback
"RTN","XWBTCPM",103,0)
 K XWB,CON,LEN,MSG ;Clean up
"RTN","XWBTCPM",104,0)
 ;Attempt to share license, Must have TCP port open first.
"RTN","XWBTCPM",105,0)
 U XWBTDEV ;D SHARELIC^%ZOSV(1)
"RTN","XWBTCPM",106,0)
 ;setup null device "NULL"
"RTN","XWBTCPM",107,0)
 S %ZIS="0H",IOP="NULL" D ^%ZIS S XWBNULL=IO I POP S XWBERROR="No NULL device" D ^%ZTER,EXIT Q
"RTN","XWBTCPM",108,0)
 D SAVDEV^%ZISUTL("XWBNULL")
"RTN","XWBTCPM",109,0)
 ;change process name
"RTN","XWBTCPM",110,0)
 D CHPRN("ip"_$P(XWBTIP,".",3,4)_":"_XWBTDEV)
"RTN","XWBTCPM",111,0)
 ;
"RTN","XWBTCPM",112,0)
RESTART ;The error trap returns to here
"RTN","XWBTCPM",113,0)
 N $ESTACK S $ETRAP="D ETRAP^XWBTCPM"
"RTN","XWBTCPM",114,0)
 S DT=$$DT^XLFDT,DTIME=30
"RTN","XWBTCPM",115,0)
 U XWBTDEV D MAIN
"RTN","XWBTCPM",116,0)
 D LOG("Exit: "_XWBTBUF)
"RTN","XWBTCPM",117,0)
 ;Turn off the error trap for the exit
"RTN","XWBTCPM",118,0)
 S $ETRAP=""
"RTN","XWBTCPM",119,0)
 D EXIT ;Logout
"RTN","XWBTCPM",120,0)
 K XWBR,XWBARY
"RTN","XWBTCPM",121,0)
 ;stop RUM for handler XWB*1.1*5
"RTN","XWBTCPM",122,0)
 D LOGRSRC^%ZOSV("$BROKER HANDLER$",2,2)
"RTN","XWBTCPM",123,0)
 D USE^%ZISUTL("XWBNULL"),CLOSE^%ZISUTL("XWBNULL")
"RTN","XWBTCPM",124,0)
 ;Close in the calling script
"RTN","XWBTCPM",125,0)
 K SOCK,TYPE,XWBSND,XWBTYPE,XWBRBUF
"RTN","XWBTCPM",126,0)
 Q
"RTN","XWBTCPM",127,0)
 ;
"RTN","XWBTCPM",128,0)
MAIN ; -- main message processing loop. debug at MAIN+1
"RTN","XWBTCPM",129,0)
 F  D  Q:XWBTBUF="#BYE#"
"RTN","XWBTCPM",130,0)
 . ;Setup
"RTN","XWBTCPM",131,0)
 . S XWBAPVER=0,XWBTBUF="",XWBTCMD="",XWBRBUF=""
"RTN","XWBTCPM",132,0)
 . K XWBR,XWBARY,XWBPRT
"RTN","XWBTCPM",133,0)
 . ; -- read client request
"RTN","XWBTCPM",134,0)
 . S XR=$$BREAD^XWBRW(1,XWBTIME,1)
"RTN","XWBTCPM",135,0)
 . I '$L(XR) D LOG("Timeout: "_XWBTIME) S XWBTBUF="#BYE#" Q
"RTN","XWBTCPM",136,0)
 . S XR=XR_$$BREAD^XWBRW(4)
"RTN","XWBTCPM",137,0)
 . I XR="#BYE#" D  Q  ;Check for exit
"RTN","XWBTCPM",138,0)
 . . D QSND^XWBRW("#BYE#"),LOG("BYE CMD") S XWBTBUF="#BYE#"
"RTN","XWBTCPM",139,0)
 . . Q
"RTN","XWBTCPM",140,0)
 . S TYPE=(XR="[XWB]")  ;check HDR
"RTN","XWBTCPM",141,0)
 . I 'TYPE D LOG("Bad Header: "_XR) Q
"RTN","XWBTCPM",142,0)
 . D CALLP^XWBPRS(.XWBR,$G(XWBDEBUG)) ;Read the NEW Msg parameters and call RPC
"RTN","XWBTCPM",143,0)
 . IF XWBTCMD="#BYE#" D  Q
"RTN","XWBTCPM",144,0)
 . . D QSND^XWBRW("#BYE#"),LOG("BYE CMD") S XWBTBUF=XWBTCMD
"RTN","XWBTCPM",145,0)
 . . Q
"RTN","XWBTCPM",146,0)
 . U XWBTDEV
"RTN","XWBTCPM",147,0)
 . S XWBPTYPE=$S('$D(XWBPTYPE):1,XWBPTYPE<1:1,XWBPTYPE>6:1,1:XWBPTYPE)
"RTN","XWBTCPM",148,0)
 . ;I $G(XWBPRT) D RETURN^XWBPRS2 Q  ;New msg return
"RTN","XWBTCPM",149,0)
 . I '$G(XWBPRT) D SND^XWBRW ;Return data,flush buffer
"RTN","XWBTCPM",150,0)
 Q  ;End Of Main
"RTN","XWBTCPM",151,0)
 ;
"RTN","XWBTCPM",152,0)
 ;
"RTN","XWBTCPM",153,0)
ETRAP ; -- on trapped error, send error info to client
"RTN","XWBTCPM",154,0)
 N XWBERC,XWBERR
"RTN","XWBTCPM",155,0)
 ;Change trapping during trap.
"RTN","XWBTCPM",156,0)
 S $ETRAP="D ^%ZTER,EXIT^XWBTCPM HALT"
"RTN","XWBTCPM",157,0)
 S XWBERC=$E($$EC^%ZOSV,1,200),XWBERR="M  ERROR="_XWBERC_$C(13,10)_"LAST REF="_$$LGR^%ZOSV
"RTN","XWBTCPM",158,0)
 I $EC["U411" S XWBERROR="U411",XWBSEC="",XWBERR="Data Transfer Error to Server"
"RTN","XWBTCPM",159,0)
 D ^%ZTER ;%ZTER clears $ZE and $ZCODE
"RTN","XWBTCPM",160,0)
 D LOG("In ETRAP: "_XWBERC) ;Log
"RTN","XWBTCPM",161,0)
 I (XWBERC["READ")!(XWBERC["WRITE")!(XWBERC["SYSTEM-F")!(XWBERC["IOEOF") D EXIT HALT
"RTN","XWBTCPM",162,0)
 U XWBTDEV
"RTN","XWBTCPM",163,0)
 I $G(XWBT("PCNT")) L ^XUTL("XUSYS",$J,0)
"RTN","XWBTCPM",164,0)
 E  L  ;Clear Locks
"RTN","XWBTCPM",165,0)
 ;I XWBOS'="DSM" D
"RTN","XWBTCPM",166,0)
 S XWBPTYPE=1 ;So SNDERR won't check XWBR
"RTN","XWBTCPM",167,0)
 ;D SNDERR^XWBRW,WRITE^XWBRW($C(24)_XWBERR_$C(4))
"RTN","XWBTCPM",168,0)
 D ESND^XWBRW($C(24)_XWBERR_$C(4))
"RTN","XWBTCPM",169,0)
 S $ETRAP="Q:($ESTACK&'$QUIT)  Q:$ESTACK -9 S $ECODE="""" D CLEANP^XWBTCPM G RESTART^XWBTCPM",$ECODE=",U99,"
"RTN","XWBTCPM",170,0)
 Q
"RTN","XWBTCPM",171,0)
 ;
"RTN","XWBTCPM",172,0)
CLEANP ;Clean up the partion
"RTN","XWBTCPM",173,0)
 N XWBTDEV,XWBNULL D KILL^XUSCLEAN
"RTN","XWBTCPM",174,0)
 Q
"RTN","XWBTCPM",175,0)
 ;
"RTN","XWBTCPM",176,0)
STYPE(X,WRAP) ;For backward compatability only
"RTN","XWBTCPM",177,0)
 I $D(WRAP) Q $$RTRNFMT^XWBLIB($G(X),WRAP)
"RTN","XWBTCPM",178,0)
 Q $$RTRNFMT^XWBLIB(X)
"RTN","XWBTCPM",179,0)
 ;
"RTN","XWBTCPM",180,0)
BREAD(L,T) ;read tcp buffer, L is length
"RTN","XWBTCPM",181,0)
 Q $$BREAD^XWBRW(L,$G(T))
"RTN","XWBTCPM",182,0)
 ;
"RTN","XWBTCPM",183,0)
CHPRN(N) ;change process name
"RTN","XWBTCPM",184,0)
 ;Change process name to N
"RTN","XWBTCPM",185,0)
 D SETNM^%ZOSV($E(N,1,15))
"RTN","XWBTCPM",186,0)
 Q
"RTN","XWBTCPM",187,0)
 ;
"RTN","XWBTCPM",188,0)
SETTIME(%) ;Set the Read timeout 0=RPC, 1=sign-on
"RTN","XWBTCPM",189,0)
 S XWBTIME=$S($G(%):90,$G(XWBVER)>1.105:$$BAT^XUPARAM,1:36000),XWBTIME(1)=2
"RTN","XWBTCPM",190,0)
 I $G(%) S XWBTIME=$S($G(XWBVER)>1.1:90,1:36000)
"RTN","XWBTCPM",191,0)
 Q
"RTN","XWBTCPM",192,0)
TIMEOUT ;Do this on MAIN  loop timeout
"RTN","XWBTCPM",193,0)
 I $G(DUZ)>0 D QSND^XWBRW("#BYE#") Q
"RTN","XWBTCPM",194,0)
 ;Sign-on timeout
"RTN","XWBTCPM",195,0)
 S XWBR(0)=0,XWBR(1)=1,XWBR(2)="",XWBR(3)="TIME-OUT",XWBPTYPE=2
"RTN","XWBTCPM",196,0)
 D SND^XWBRW
"RTN","XWBTCPM",197,0)
 Q
"RTN","XWBTCPM",198,0)
 ;
"RTN","XWBTCPM",199,0)
OS() ;Return the OS
"RTN","XWBTCPM",200,0)
 ; Q $S(^%ZOSF("OS")["DSM":"DSM",^("OS")["UNIX":"UNIX",^("OS")["OpenM":"OpenM",1:"MSM") //SMH
"RTN","XWBTCPM",201,0)
 Q $S(^%ZOSF("OS")["DSM":"DSM",^("OS")["GT.M":"GT.M",^("OS")["OpenM":"OpenM",1:"MSM")
"RTN","XWBTCPM",202,0)
 ;
"RTN","XWBTCPM",203,0)
INIT ;Setup
"RTN","XWBTCPM",204,0)
 S U="^",XWBTIME=10,XWBOS=$$OS,XWBDEBUG=0,XWBRBUF=""
"RTN","XWBTCPM",205,0)
 S XWBDEBUG=$$GET^XPAR("SYS","XWBDEBUG")
"RTN","XWBTCPM",206,0)
 S XWBT("BF")=$S(XWBOS="GT.M":"#",1:"!")
"RTN","XWBTCPM",207,0)
 S XWBT("PCNT")=0 I XWBOS="GT.M",$L($T(^XUSCNT)) S XWBT("PCNT")=1
"RTN","XWBTCPM",208,0)
 D LOGSTART^XWBDLOG("XWBTCPM")
"RTN","XWBTCPM",209,0)
 Q
"RTN","XWBTCPM",210,0)
 ;
"RTN","XWBTCPM",211,0)
DEBUG ;Entry point for debug, Build a server to get the connect
"RTN","XWBTCPM",212,0)
 ;DSM sample;ZDEBUG ON S $ZB(1)="SERV+1^XWBTCPM:1",$ZB="ETRAP+1^XWBTCPM:1"
"RTN","XWBTCPM",213,0)
 W !,"Before running this entry point set your debugger to stop at"
"RTN","XWBTCPM",214,0)
 W !,"the place you want to debug. Some spots to use:"
"RTN","XWBTCPM",215,0)
 W !,"'SERV+1^XWBTCPM', 'MAIN+1^XWBTCPM' or 'CAPI+1^XWBPRS.'",!
"RTN","XWBTCPM",216,0)
 W !,"or location of your choice.",!
"RTN","XWBTCPM",217,0)
 W !,"IP Socket to Listen on: " R SOCK:300 Q:'$T!(SOCK["^")
"RTN","XWBTCPM",218,0)
 ;Use %ZISTCP to do a single server
"RTN","XWBTCPM",219,0)
 D LISTEN^%ZISTCP(SOCK,"SERV^XWBTCPM")
"RTN","XWBTCPM",220,0)
 U $P W !,"Done"
"RTN","XWBTCPM",221,0)
 Q
"RTN","XWBTCPM",222,0)
SERV ;Callback from the server
"RTN","XWBTCPM",223,0)
 S XWBTDEV=IO,XWBTIME(1)=3600 D INIT
"RTN","XWBTCPM",224,0)
 S XWBDEBUG=1,MSG=$$BREAD^XWBRW(5,60) ;R MSG#5
"RTN","XWBTCPM",225,0)
 D NEW
"RTN","XWBTCPM",226,0)
 S IO("C")=1 ;Cause the Listenr to stop
"RTN","XWBTCPM",227,0)
 Q
"RTN","XWBTCPM",228,0)
 ;
"RTN","XWBTCPM",229,0)
EXIT ;Close out
"RTN","XWBTCPM",230,0)
 I $G(DUZ) D LOGOUT^XUSRB
"RTN","XWBTCPM",231,0)
 I $G(XWBT("PCNT")) D COUNT^XUSCNT(-1)
"RTN","XWBTCPM",232,0)
 Q
"RTN","XWBTCPM",233,0)
 ;
"RTN","XWBTCPM",234,0)
LOG(MSG) ;Record Debug Info
"RTN","XWBTCPM",235,0)
 D:$G(XWBDEBUG) LOG^XWBDLOG(MSG)
"RTN","XWBTCPM",236,0)
 Q
"RTN","XWBTCPM",237,0)
 ;
"VER")
8.0^22.0
**END**
**END**
