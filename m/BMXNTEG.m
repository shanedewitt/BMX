BMXNTEG	;INTEGRITY CHECKER;FEB 26, 2007
	;;2.3;BMX;;Jan 25, 2011
	;
START	;
	NEW BYTE,COUNT,RTN
	K ^UTILITY($J)
	F I=1:1 S X=$T(LIST+I) Q:X=""  S X=$P(X,";;",2),R=$P(X,"^",1),B=$P(X,"^",2),C=$P(X,"^",3),^UTILITY($J,R)=B_"^"_C
	F I=1:1:6 S X=$P($T(@("LINE"_I)),";;",2,99),@("XBSUMBLD("_I_")=X")
	X XBSUMBLD(1)
	Q
	;
LINE1	;;X XBSUMBLD(2),XBSUMBLD(6)
LINE2	;;S RTN=0 F  S RTN=$O(^UTILITY($J,RTN)) Q:RTN=""  W !,RTN ZL @RTN S (BYTE,COUNT)=0 S X=$T(+1),X=$P(X," [ ",1) X XBSUMBLD(4),XBSUMBLD(3),XBSUMBLD(5)
LINE3	;;F I=2:1 S X=$T(+I) Q:X=""  X XBSUMBLD(4)
LINE4	;;F J=1:1 S Y=$E(X,J) Q:Y=""  S BYTE=BYTE+1,COUNT=COUNT+$A(Y)
LINE5	;;S B=$P(^UTILITY($J,RTN),"^",1),C=$P(^(RTN),"^",2) I B'=BYTE!(C'=COUNT) W "  has been modified"
LINE6	;;K XBSUMBLD,B,C,I,J,R,X,Y
	;
LIST	;
	;;BMXADE1^3028^202865
	;;BMXADE2^3250^215372
	;;BMXADO^6547^418026
	;;BMXADO2^3489^255546
	;;BMXADOF^11562^731974
	;;BMXADOF1^3281^207224
	;;BMXADOF2^2138^139496
	;;BMXADOFD^2831^178610
	;;BMXADOFS^6515^393782
	;;BMXADOI^2215^134605
	;;BMXADOS^9145^575000
	;;BMXADOS1^2590^161592
	;;BMXADOV^5739^373823
	;;BMXADOV1^9072^554887
	;;BMXADOV2^4690^289898
	;;BMXADOVJ^3530^225534
	;;BMXADOX^13904^870277
	;;BMXADOX1^11753^751110
	;;BMXADOX2^3126^199406
	;;BMXADOXX^12226^762799
	;;BMXADOXY^11992^769511
	;;BMXE01^2111^148783
	;;BMXFIND^7919^562996
	;;BMXG^1970^120467
	;;BMXGETS^4309^308726
	;;BMXMBRK^5919^389568
	;;BMXMBRK2^3621^233089
	;;BMXMEVN^6627^468908
	;;BMXMON^9356^664477
	;;BMXMSEC^2302^160584
	;;BMXNTEG^2045^127438
	;;BMXPO^1522^101987
	;;BMXPRS^2153^134429
	;;BMXRPC^5716^425699
	;;BMXRPC1^7622^559198
	;;BMXRPC2^3531^243875
	;;BMXRPC3^6466^450166
	;;BMXRPC4^4967^312485
	;;BMXRPC5^3896^288926
	;;BMXRPC6^3757^270667
	;;BMXRPC7^5687^404431
	;;BMXRPC8^2236^165523
	;;BMXRPC9^6408^421855
	;;BMXSQL^10869^727499
	;;BMXSQL1^9921^616204
	;;BMXSQL2^2748^183754
	;;BMXSQL3^13516^868578
	;;BMXSQL4^1313^88477
	;;BMXSQL5^6648^433290
	;;BMXSQL6^10606^683062
	;;BMXSQL7^8102^528283
	;;BMXSQL91^4328^281351
	;;BMXTABLE^159^9961
	;;BMXTRS^1300^81264
	;;BMXUTL1^7818^520369
	;;BMXUTL2^900^60457
	;;BMXUTL5^5330^358866
	;;BMXUTL6^942^62126
	;;BMXUTL7^163^10646
