BMXMSEC	; IHS/OIT/HMW - BMXNet MONITOR ; 7/20/2009
	;;2.2;BMX;;Sep 07, 2010
	; Edit History
	; Line SETUP+2 has been changed to support GT.M //SMH 7/5/09
	; Per Wally Fort's GT.M code in XWBTCPM, IP for GT.M is stored
	; in IP("GTM-IP"). Changes in BMXMON and here follow that model.
	;
CHKPRMIT(BMXRP)	;EP - checks to see if remote procedure is permited to run
	;Input:  BMXRP - Remote procedure to check
	Q:$$KCHK("XUPROGMODE")
	N ERR,BMXALLOW
	S U="^",BMXSEC="" ;clear
	;
	;In the beginning, when no DUZ is defined and no context exist, setup
	;default signon context
	S:'$G(DUZ) DUZ=0,XQY0="XUS SIGNON"   ;set up default context
	;
	I BMXRP'="XWB IM HERE",BMXRP'="XWB CREATE CONTEXT",BMXRP'="XWB RPC LIST",BMXRP'="BMX AV CODE" D  ;check exemptions. new exemption for XWB*1.1*6 - dpc
	. I $G(XQY0)'="" D
	. . S BMXALLOW=$$CHK^XQCS(DUZ,$P(XQY0,U),BMXRP)         ;do the check
	. . S:'BMXALLOW BMXSEC=BMXALLOW
	. E  S BMXSEC="Application context has not been created!"
	Q
	;
OWNSKEY(RET,LIST)	;EP Does user have Key
	N I,K S I=""
	I $G(DUZ)'>0 S RET(0)=0 Q
	I $O(LIST(""))="" S RET(0)=$$KCHK(LIST) Q
	F  S I=$O(LIST(I)) Q:I=""  S RET(I)=$$KCHK(LIST(I))
	Q
KCHK(%)	Q $S($G(DUZ)>0:$D(^XUSEC(%,DUZ)),1:0) ;EP Key Check
	;
	;
SETUP(RET)	;EP - sets up environment for GUI signon
	;
	K ^TMP("XQCS",$J)
	; S IO("IP")=$P D ZIO^%ZIS4 ;IHS/OIT/HMW SAC Exemption Applied For
	; --> Begin new code
	I $$OS^XWBTCPM="GT.M" S IO("IP")=IO("GTM-IP")
	I $$OS^XWBTCPM="OpenM" S IO("IP")=$P
	D ZIO^%ZIS4
	; <-- End new code //SMH
	D SET1(0),SET^BMXMSEC("XUS XOPT",XOPT),SET^BMXMSEC("XUS CNT",0)
	S %ZIS="0H",IOP="NULL" D ^%ZIS
	;0=server name, 1=volume, 2=uci, 3=device, 4=# attempts, 5=skip signon-screen
	S RET(0)=$P(XUENV,U,3),RET(1)=$P(XUVOL,U),RET(2)=XUCI
	S RET(3)=$I,RET(4)=$P(XOPT,U,2),RET(5)=0 ;IHS/OIT/HMW SAC Exemption Applied For
	I $$INHIBIT() Q
	Q
	;
SET1(FLAG)	;Setup parameters
	D GETENV^%ZOSV S U="^",XUENV=Y,XUCI=$P(Y,U,1),XQVOL=$P(Y,U,2),XUEON=^%ZOSF("EON"),XUEOFF=^("EOFF")
	S X=$O(^XTV(8989.3,1,4,"B",XQVOL,0)),XUVOL=$S(X>0:^XTV(8989.3,1,4,X,0),1:XQVOL_"^y^1") S:$P(XUVOL,U,6)="y" XRTL=XUCI_","_XQVOL
	S XOPT=$S($D(^XTV(8989.3,1,"XUS")):^("XUS"),1:"") F I=2:1:15 I $P(XOPT,U,I)="" S $P(XOPT,U,I)=$P("^5^900^1^1^^^^1^300^^^^N^90",U,I)
	Q
	;
INHIBIT()	;Is Logon to this system Inhibited?
	I $G(^%ZIS(14.5,"LOGON",XQVOL)) Q 1
	I $D(^%ZOSF("ACTJ")) X ^("ACTJ") I $P(XUVOL,U,3),($P(XUVOL,U,3)'>Y) Q 2
	Q 0
	;
NOW	S U="^",XUNOW=$$NOW^XLFDT(),DT=$P(XUNOW,"."),XUDEV=0
	Q
	;
STATE(%)	;Return a state value
	;XWBSTATE is required by XUSRB
	Q:'$L($G(%)) $G(XWBSTATE)
	Q $G(XWBSTATE(%))
	;
	;
SET(%,VALUE)	;Set the state variable
	I $G(%)="" S XWBSTATE=VALUE
	S XWBSTATE(%)=VALUE
	Q
KILL(%)	;Kill state variable
	I $L($G(%)) K XWBSTATE(%)
	Q
