BMXMBRK2	; IHS/OIT/HMW - BMXNet MONITOR ;
	;;2.31;BMX;;Jul 25, 2011
	;
	;
CAPI(BMXY,TAG,NAM,PAR)	;EP - make API call
	N R,T,DX,DY
	IF BMXZ(1,"FLAG")=2 D
	. S PAR=$P(PAR,BMXZ("FRM"))_BMXZ("TO")_$P(PAR,BMXZ("FRM"),2)
	S R=$S(PAR'=+PAR&(PAR=""):TAG_"^"_NAM_"(.BMXY)",1:TAG_"^"_NAM_"(.BMXY,"_PAR_")")
	U IO
	D @R
	; D DEBUG^%Serenji("@R","10.10.10.104")
	U $P
	Q
	;
BHDR(WKID,WINH,PRCH,WISH)	;Build a protocol header
	N S,L
	S S=""
	S S=WKID_";"_WINH_";"_PRCH_";"_WISH_";"
	S L=$L(S)
	S S=$E("000"_L,$L(L)+1,$L(L)+3)_S
	Q S
	;
BARY(A,R,V)	;add array elements+values to storage array
	IF A'["BMXS" Q "-1^ARRAY NAME MUST BE BMXS"
	S @A@(R)=V
	Q 0
	;
BLDB(P)	;Build formatted string
	N L
	S L=$L(P)
	Q $E("000"_L,$L(L)+1,$L(L)+3)_P
	;
BLDA(N,P)	;Build API string
	;M Extrinsic Function
	;Inputs
	;N        API name
	;P        Comma delimited parameter string
	;Outputs
	;String   API string if successful, "-1^Text" if error
	;
	N I,F,L,T,U,T1,T2
	IF '+$D(N) Q "-1^Required input reference is NULL"
	S U="^"
	S (F,T,Y)=0
	IF '$D(P) S P=""
	IF P'="" D
	. S L=$L(P)-$L($TR(P,$C(44)))+1
	. IF L=0 S L=1
	. F I=1:1:L D  Q:T
	. . S T1=$P(P,",",I)
	. . S T2=$E(T1,1,1)="."
	. . IF T1=+T1 Q
	. . IF $E(T1,1,1)="^" S F=2,T=1 Q
	. . IF T2&($E(T1,2,$L(T1))?.ANP) S F=1,T=1 Q
	S P=$$BLDB(P)
	S L=$L(P)+$L(P)-3
	S P=F_N_U_P
	S L=$L(P)
	Q $E("000"_L,$L(L)+1,$L(L)+3)_P
	;
BLDS(R)	;Build a parameter string from an array
	N L,T,Y
	S Y=""
	F  D  Q:R=""
	. S R=$Q(@R)
	. IF R="" Q
	. S L=$L(R)+$L(@R)+1
	. S T=@R
	. S T=$TR(T,$C(44),$C(23))
	. S Y=Y_$E("000"_L,$L(L)+1,$L(L)+3)_R_"="_T
	Q Y_"000"
	;
BLDU(R)	;Build a parameter string from a scalar
	N DONE,L,N,N1,P1
	IF R=+R Q R
	S N=$F(R,$C(34))
	IF N=0 Q $C(34)_R_$C(34)
	S P1=$E(R,1,N-2)
	S (L,DONE)=0
	F  D  Q:DONE
	. S N1=$F(R,$C(34),N)
	. IF N1=0 S L=$L(R)+2,N1=L
	. S P1=P1_$C(34,34)_$E(R,N,N1-2)
	. IF N1=L S DONE=1,P1=$C(34)_P1_$C(34) Q
	. S N=N1
	Q $TR(P1,$C(44),$C(23))
	;
BLDG(R)	;build a parameter string from a global reference
	N I,L,L1,M,T,T1,T2,Y
	K ^TMP("BMXZ",$J)
	IF '$D(R) Q "-1^Reference does not exist"
	S Y=$NA(^TMP("BMXZ",$J,$P($H,",",2)))
	S I=0
	S M=512
	S T1=$P(R,")")
	S L1=$L($P(R,"("))
	F  D  Q:R=""
	. S R=$Q(@R)
	. S T2=$F(R,"(")
	. IF R=""!(R'[T1) Q
	. S L=$L(R)+$L(@R)-L1
	. S T=@R
	. S T=$TR(T,$C(44),$C(23))
	. S @Y@(I)=$E("000"_L,$L(L)+1,$L(L)+3)_"^("_$E(R,T2,M)_"="_$$BLDU(T)
	. S I=I+1
	S @Y@(I)="000"
	S Y=$TR(Y,$C(44),$C(23))
	Q Y
	;
OARY()	;EP - create storage array
	N A,DONE,I
	S (DONE,I)=0
	F I=1:1 D  Q:DONE
	. S A="BMXS"_I
	. K @A ;temp fix for single array
	. IF '$D(@A) S DONE=1
	S @A="" ;set naked
	Q A
	;
CREF(R,P)	;EP - Convert array contained in P to reference A
	N I,X,DONE,F1,S
	S DONE=0
	S S=""
	F I=1:1  D  Q:DONE
	. IF $P(P,",",I)="" S DONE=1 Q
	. S X(I)=$P(P,",",I)
	. IF X(I)?1"."1A.E D
	. . S F1=$F(X(I),".")
	. . S X(I)="."_R
	. S S=S_X(I)_","
	Q $E(S,1,$L(S)-1)
	;
GETP(P)	;returns various parameters out of the Protocol string
	N M,T,BMXZ
	S M=512
	S T=$$PRSP^BMXMBRK(P)
	IF '+T D
	. S T=$$PRSM^BMXMBRK(BMXZ(0,"MESG"))
	. IF '+T S T=BMXZ(0,"WKID")_";"_BMXZ(0,"WINH")_";"_BMXZ(0,"PRCH")_";"_BMXZ(0,"WISH")_";"_$P(BMXZ(1,"TEXT"),"^")
	Q T
	;
CALLM(X,P,DEBUG)	;make call using Message string
	N ERR,S
	S X="",ERR=0
	S ERR=$$PRSM^BMXMBRK(P)
	IF '+ERR S ERR=$$PRSA^BMXMBRK(BMXZ(1,"TEXT"))
	IF '+ERR S S=$$PRSB^BMXMBRK(BMXZ(2,"PARM"))
	IF (+S=0)!(+S>0) D
	. D CAPI(.X,BMXZ(2,"RTAG"),BMXZ(2,"RNAM"),S)
	IF 'DEBUG K BMXZ
	K @(X("BMXS")),X("BMXS")
	Q
	;
CALLA(X,P,DEBUG)	;make call using API string
	N ERR,S
	S X="",ERR=0
	S ERR=$$PRSA^BMXMBRK(P)
	IF '+ERR S S=$$PRSB^BMXMBRK(BMXZ(2,"PARM"))
	IF (+S=0)!(+S>0) D
	. D CAPI(.X,BMXZ(2,"RTAG"),BMXZ(2,"RNAM"),S)
	IF 'DEBUG K BMXZ
	K @(X("BMXS")),X("BMXS")
	Q
	;
TRANSPRT()	;Determine the Transport Method
	;DDP is local :=0
	;TCP/IP is remote :=1
	;Serial/RS-232 is remote :=2
	Q 1
	;Q 0 ;Do DDP for Now
