BMXE01	; IHS/OIT/FJE - ENVIRONMENT CHECK FOR BMX 2.0 ;
	;;2.1;BMX;;Jul 26, 2009
	;
	S $P(LINE,"*",81)=""
	S XPDNOQUE="NO QUE"  ;NO QUEUING ALLOWED
	S XPDABORT=0
	I '$G(DUZ) W !,"DUZ UNDEFINED OR 0." S XPX="DUZ" D SORRY Q
	;
	I '$L($G(DUZ(0))) W !,"DUZ(0) UNDEFINED OR NULL." S XPX="DUZ" D SORRY Q
	;
	D HOME^%ZIS,DT^DICRW
	S X=$P($G(^VA(200,DUZ,0)),U)
	I $G(X)="" W !,"Who are you????" S XPX="DUZ" D SORRY Q
	W !,"Hello, "_$P(X,",",2)_" "_$P(X,",")
	W !!,"Checking Environment for Install of Version "_$P($T(+2),";",3)_" of "_$P($T(+2),";",4)_"."
	;
	S X=$G(^DD("VERSION"))
	W !!,"Need at least FileMan 22.....FileMan "_X_" Present"
	I X<22 S XPX="FM" D SORRY Q
	;
	S X=$G(^DIC(9.4,$O(^DIC(9.4,"C","XU",0)),"VERSION"))
	W !!,"Need at least Kernel 8.0.....Kernel "_X_" Present"
	I +X<8 S XPX="KERNEL" D SORRY Q
	;
	S X=$G(^DIC(9.4,$O(^DIC(9.4,"C","XB",0)),"VERSION"))
	W !!,"Need at least XB/ZIB 3.....XB/ZIB "_X_" Present"
	I +X<2 S XPX="XB" D SORRY Q
	q
ENVOK	; If this is just an environ check, end here.
	W !!,"ENVIRONMENT OK." 
	;
	; The following line prevents the "Disable Options..." and "Move
	; Routines..." questions from being asked during the install.
	I $G(XPDENV)=1 S (XPDDIQ("XPZ1"),XPDDIQ("XPZ2"))=0
	I $G(XPDENV)=1 D  ;Updates BMX Version file 
	.S X="2",DIC="^BMXAPPL(",DLAYGO=90093.2,DIC(0)="E" K DD,D0 D FILE^DICN
	.S DA=+Y
	.S:+DA DIE="^BMXAPPL(",DR=".02///0;.03////"_DT D ^DIE
	.K DIE,DA
	Q
SORRY	;
	K DIFQ
	S XPDABORT=1
	W *7,!!!,"Sorry....something is wrong with your environment"
	W !,"Aborting BMX Version 2.0 Install!"
	W !,"Correct error and reinstall otherwise"
	W !,"please print/capture this screen and notify"
	W !,"technical support."
	W !!,LINE
	D BMES^XPDUTL("Sorry....something is wrong with your environment")
	D BMES^XPDUTL("Enviroment ERROR "_$G(XPX))
	D BMES^XPDUTL("Aborting BMX 2.0 install!")
	D BMES^XPDUTL("Correct error and reinstall otherwise")
	D BMES^XPDUTL("please print/capture this screen and notify")
	D BMES^XPDUTL("technical support.")
	Q
	;
