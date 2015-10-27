# Team Foundation Dev Tools Utility

Home Page : http://ablaze8.github.io/TeamFoundationDevTools/ <br/>
My Blog   : https://wildclick.wordpress.com/ 

This is a Free ( as in beer ! ) & Open Source utility software distributed under GNU GPL v2. This software is running on .NET platform 4.5.2 ( or alternativly please switch to [DotNet35](https://github.com/ablaze8/TeamFoundationDevTools/tree/DotNet35) branch for compatibility )

### Additional License Info :

- You're free to use for personal / commercial use
- You cannot incorporate GPL-covered software in a proprietary system.
- You need my permission to release your enhancement under a different license
- You must share your modifications under the same license ( unless a written persmission from myself )
- You're more than welcomed to join the development efforts and add/enhance features by sending me pull requests


### Planned Features :


#### 1.	Auto Detect
--------------
- detects and lists all pre configured TFS connections and gives the opportunity to select / switch.
- also let's you manually connect to a connection ( just in the case )

#### 2.	Recusrive Find
--------------
- all search results accompnied by a .txt file which contains more details
- finds for a particular WildCard search for each file name for every project
- you may also narrow down the results for a prticular sub-string in a path to that file
- screen output has color codded output to differentiate between no match, match and path filter results
- if your TFS host projects built in diffrent versions of Visual Studio, the search results also suggest which one to use !
- every match is displayed along with Last Check-in Date, Commited By, Changeset# & optionally check in comment 
 - this info is also available in text file

##### Preferences :

- search results may be directed to screen and / or a txt file
- you  have the choice to choose between summary and detailed output.
- you have granular control over switching between summarized / detailed output for screen / txt output
