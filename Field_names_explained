This document details the contents of the ramification check results. The program receives as input a list of ramification data that may appear as the ramification data of an extension $\Omega/\mathbb{C}(t)$ with Galois group $A_n^2\leq G \leq S_n \wr S_2$.
**Notation:**
 In what follows,  $S_n \wr S_2:=(S_n\times S_n) \rtimes S_2$, where $S_2$ acts on $S_n\times S_n$ by permuting the coordinates; similarly $A_n \wr S_2:=(A_n\times A_n)\rtimes S_2$; If two groups $H_1,H_2$ each have a unique projection onto a group $Q$, then we denote by $H_1\times_{Q}H_2$ indicates the fiber product of these projections.  "type names" implies the name given to the type in the classification of primitive monodromy groups (Neftin-Zieve, Guralnick-Shareshian).


**Wreath Type** the corresponding type name in the input.
**Mon** the monodromy group of the type. This is calculated via the following procedure:
* Identify $S_n \wr S_2/ A_n\times A_n$ with $C_2\wr C_2$ (dihedral group of order 8).
* calculate the image of each generator (element in data) $\mod A_n\times A_n$. 
* calculate the minimal subgroup of $C_2\wr C_2$ containing these images.
(this method has a problem since ramification data is only given up to $S_n\wr S_2$ conjugacy, however for our list of input it works OK, see Lemma 10.3 of "monodromy groups of product type" article. 

**n substitution** assumptions on $n$. If $n=r\mod k$ then the program substitutes $r$ in $n$ to determine parity of expressions involving $n$.
**a substitution** assumptions on $a$. If $a=r\mod k$ then the program substitutes $r$ in $a$ to determine parity of expressions involving $a$. 
*Remark:* $n$ and $a$ are always considered modulo the same $k$.
**assumptions** a string formatting of n assumptions and a assumptions together.

**Sn-1xSn-1 contribution** calculation of the Riemann-Hurwitz contribution in $\Omega^{S_{n-1}^2\cap G}/\Omega^{S_{n-1}\wr S_2}$ - use this to calculate $g_{S_{n-1}^2}$ manually form the Riemann-Hurwitz formula + existing information on $g_{S_{n-1}\wr S_2}$  (by Remark 3.3 in the "Monodromy groups of product type" article: $g_{S_{n-1}\wr S_2}=0$ except for types I1A.1-I1A.3 and types F4.*, for which $g_{S_{n-1}\wr S_2}=1$.) We don't have a calculation of $g_{S_{n-1}\wr S_2}$ in this version so $g_{S_{n-1}^2}$ needs to be calculated manually. 

**Kernel type** the type name (if applicable) of the ramification data of the symmetric\alternating extension obtained by projecting the ramification data of $\Omega/\Omega^{S_n^2}$ to the right hand side\left hand side coordinate. I.e., we calculate this ramification data and match it against the table of exceptional ramification data for symmetric extensions. 
**(Sn # Sn) wr S2 type** if the ramification data of $\Omega/\Omega^{(S_n\times_{C_2}S_n)\rtimes C_2}$ is also exceptional, the name of the type is given here.
**AnC4 type** the type (if exceptional) of the ramification data of $\Omega/\Omega^{A_n^2.C_4}$.
**Sn#Sn type** the type (if exceptional) of the ramification data of the symmetric extension obtained by projecting the ramification data of $\Omega^{S_n\times_{C_2}S_n}$ to the left hand side coordinate.
**SnAn type** the type (if exceptional) of the ramification data of tahe symmetric extension obtained by projecting the ramification data of $\Omega/\Omega^{S_n\times A_n}$ to the left hand side coordinate.
**AnAn type** the type (if exceptional) of the ramification data of the alternating extension obtained by projecting the ramification data of $\Omega/\Omega^{A_n^2}$ to the left hand side coordinate.

The following ten column names are values of an enum describing the ten subgroups $A_n^2\leq G \leq S_n \wr S_2$ - here we give the corresponding math notations. These are also the possible values for the **Mon** column. These eight columns are only filled in when $G=Mon=S_n \wr S_2$ and their contents give $g_{H}$ for the low index subgroups $A_n^2\leq H \leq S_n wr S_2$:
**SnWrS2**  this is  $S_n\wr S_2$ - the content should always be 0.
**SnSquared**, this is the ``kernel" subgroup $S_n^2$. This genus is also easily read off the ramification data by considering the number of elements containing a swap.
**AnC4** this is the subgroup $A_n^2.C_4$.
**FiberWrS2** this is the subgroup $(S_n\times_{C_2}S_n)\rtimes C_2$.
**Fiber** this is the subgroup $S_n\times_{C_2}S_n$.
**AnxSn**.
**SnxAn**.
**AnxAn**.
**AnWrS2** this is the subgroup $A_n \wr S_2$.
**AnWrS2Cong** this is the subgroup $\langle A_n^2, (\tau,\tau)s \rangle$, $\tau\in S_n$ a transposition (a conjugate of $A_n \wr S_2$).

Some of the genera above are calculated again using a slightly different method:
**Kernel genus** - the genus of the subfield fixed by $S_n^2$, calculated by counting the number of elements with a swap + Riemann-Hurwitz formula.
**Sn # Sn genus** - the genus of the subfield fixed by $S_n\times_{C_2}S_n$, calculated by counting the number of elements in the $S_n^2$ data with different signs in the RHS and LHS coordinates + Riemann-Hurwitz formula.
**SnAn genus** - calculated by counting the number of elements in the $S_n^2$ data with odd RHS + Riemann-Hurwitz formula.
**AnAn genus** - calculated by counting the number of elements in the $S_n\times_{C_2}S_n$ data with odd RHS + Riemann-Hurwitz formula. 

The following columns contain genera of subfields fixed by subgroups $A_n\times A_{n-1}\leq H \leq S_{n-1}\times S_n$:
**pt stab genus** the genus of the subfield fixed by $S_{n-1}\times S_n\cap G$
**S_n-1 x An genus**  the genus of the subfield fixed by $S_{n-1}\times A_n$ (applicable  only when $G=S_n wr S_2$).
**SnAn-1 genus** the genus of the subfield fixed by $S_n\times A_{n-1}$ (applicable only when $G= S_n wr S_2$). 
**Sn # Sn-1 genus** $S_n\times_{C_2}S_{n-1}$ the genus of the subfield fixed by $S_{n-1}\times_{C_2}S_n$ - this column is filled in only when $S_{n-1}\times_{C_2}S_n\neq G\cap S_{n-1}\times S_n$. This should have 0 or 1 if there's an Sn#Sn type.
**AnAn-1 genus** the genus of the subfield fixed by $A_{n-1}\times A_n$ - this column is filled in only when $A_{n-1}\times A_n\neq G\cap S_{n-1}\times S_n$, i.e., when $G\neq A_n wr S_2$.

**an wr s2 match** the ramification data (if exceptional) of the $\Omega/\Omega^{A_n wr S_2}$. 
**an wr s2 genus** ignore this field (these are originally calculations for the genus of subfields fixed by $A_n\wr S_2$ in $(S_n\times_{C_2}S_n)\rtimes C_2$-extensions but these needed to be worked out manually as the ramification data in input is only up to $S_n wr S_2$-conjugacy).

The following two columns are the result of a verification done by the program 
**check** this field does nothing, ignore.
**accola check** verification that Accola's formula holds for the genera calculated for subfields of the Klein extension $\Omega^{A_{n-1}\times A_n}/\Omega^{S_{n-1}\times S_n}$. 

The following columns deal with subfields fixed by subgroups $A_{n-2}\times A_n\leq H \leq S_{n-2}\times S_2\times S_n$:
**2 set genus kernel** the genus of the subfield fixed by $(S_{n-2}\times S_2)\times S_n\cap G$.
**2 pt genus** the genus of the subfield fixed by $S_{n-2}\times S_n\cap G$.

The following genus calculations are only applicable when $Mon = S_n wr S_2$: 
**kernel s2 x an-2 genus** the genus of the subfield fixed by $A_{n-1}\times S_2\times S_n$
**Sn # Sn 2 set stab genus** the genus of the subfield fixed by $S_n\times_{C_2}S_{n-2}$
**Sn x An rhs 2set genus** the genus of the subfield fixed by $S_n\times (S_{n-2}\times_{C_2}S_2)$ (this is equal to the genus of the subfield fixed by $(S_{n-2}\times _{C_2}S_2)\times S_n$).
**Sn x An lhs 2set genus** the genus of the subfield fixed by $S_{n-2}\times S_2\times A_n$.
**S_{n-2}xAn** the genus of the subfield fixed by $S_{n-2}\times A_n$.
**S_2xA_{n-2}xA_n** the genus of the subfield fixed by $A_{n-2}\times S_2\times A_n$.
**AnxAn 2set genus** the genus of the subfield fixed by $(S_{n-2}\times_{C_2}S_2)\times A_n$.
**S2#Sn-2#Sn** the genus of the subfield fixed by the fiber product $S_n\times_{C_2}(S_{n-2}\times_{C_2}S_2)$ whose kernel is $A_n\times A_{n-2}$ (calculated from using Accola's formula). You can also think of this subgroup as $S_n\times A_{(2)}\cap S_n\times_{C_2}S_n$, where $A_{(2)}$ is a $2$-set stabilizer of $A_n$. 
**a_{n-2}xS_n** the genus of the subfield fixed by $A_{n-1}\times S_n$.
**special1** the genus of the subfield fixed by the fiber product $(S_2\times S_{n-2})\times_{C_2}S_n$ whose kernel is $S_2\times A_{n-2}\times A_n$, i.e., $S_2\times(S_{n-2}\times_{C_2}S_n)$ (calculated using Accola's formula).
**special2** the genus of the subfield fixed by the fiber product $(S_2\times S_{n-2})\times_{C_2}S_n$ whose kerenel is $S_{n-2}\times A_n$, i.e., this subgroup is isomorphic to $S_{n-2}\times(S_2\times_{C_2}S_n)$. 
**special3** the genus of the subfield fixed by the fiber product of special1 and $S_n$, that is, the fiber product  $S_2\times_{C_2}(S_{n-2}\times_{C_2}S_n)$ whose kerenel is $A_{n-2}\times A_n$.
**sn-2#sn** the genus of the subfield fixed by $S_{n-2}\times_{C_2}S_n$. 
