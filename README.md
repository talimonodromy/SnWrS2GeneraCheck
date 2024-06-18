
<p><strong>SnWrS2Ramification.sln</strong> - project written in C# in Microsoft Visual Studio Community 2019.
 the framework <a href="https://symbolics.mathdotnet.com/">Math.NET Symbolics</a>  is used for symbolic calculation.</p>
<p><strong>RamificationTests</strong> project: contains unit tests. 
<strong>SnS2RamificationCheck</strong> project:  contains the genus calculations. Target framework: .net 5.0
        Program.cs - main class
<strong>Output:</strong>
Running the SnS2RamificationCheck project outputs several files:
*- results_ddmmyy_hhmmss.csv contains the results of the genus calculations in csv format.</p>
<ul>
<li>the other files contain various results outputted in a format ready for copying and pasting into a latex table (generated using the function PrintColumnsToLatexTable).</li>
</ul>
<p><strong>Input:</strong></p>
<ul>
<li>The table of exceptional wreath types is stored in Input\WreathTypes.txt </li>
<li>the table of exceptional symmetric types is stored in Input\SymmetricTypesLatex.txt</li>
</ul>
<p><strong>Objects:</strong>        </p>
<ul>
<li>an individual branch cycle of an Sn-extension is modeled by the class SnBranchCycle.</li>
<li>the ramification data of an Sn-extension is modeled by the class SnRamificationType, which wraps a list of SnBranchCycle objects.</li>
<li>a partition of a parameter n is modeled by a list of PartitionPart classes.</li>
<li><p>a PartitionPart class contains two symbolic expressions: Part and Times.</p>
</li>
<li><p>a branch cycle of an Sn wr s2 extension is modeled by the class     - SnWrS2BranchCycleReducedForm.    </p>
</li>
<li>the ramification data of an sn wr s2 extension is modeled by the class SnWrS2RamificationType, which wraps a list of SnWrBranchCycleReducedForm objects.</li>
</ul>
<p>Care must be taken in comparing these objects - always note whether or not the equality of expressions needs to be evaluated symbolically. </p>
sns2check_documentation.html
Displaying sns2check_documentation.html.
