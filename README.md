# CoinWars
The game

Demo Videos:<br />
https://www.youtube.com/watch?v=qOuIEyAAFgM<br />
https://www.youtube.com/watch?v=Lzvdf_0ps0s<br />
(many many more. can trace history of this game by watching the vids my YT account!)


Ok likely many unnecessary/stale files in here...here's some navigation in Unity3D--Checked with Unity5<br />
1) Latest game scene: Assets>testConfigs<br />
2) reflex AI scene: Assets>AI>AI

**In Depth Rules/Gameplay:**
tldr version: 2 players control 2 fleets of red or yellow collored coins and have the coins fight each other untill all coins of one color have "died".

How to kill a coin:
There are two ways to kill a coin:<br />
1) Note that each coin has an energy bar (the halo around it). As coins incur damage (inflicted collisions by other coins and especially so by said coins equipped by the Dynamite powerup!) this bar disappears.  Once the halo is gone, the coin dies.<br />
<br>
<img src="https://dl.dropboxusercontent.com/u/105935968/CoinWars/Snapshots/Healthy.png" alt="orignal"> 
<br>
<img src="https://dl.dropboxusercontent.com/u/105935968/CoinWars/Snapshots/Damaged.png" alt="orignal"> 
<br>
2) Knock the coin off the platform. It will die.


Features:
Currency Exchange, Powerups, Terrain Teleporting

**Currency Exchange:** Merge and split coins by "D" key

**Merging:**

Say we have three unlocked coins "near" each other (near = experiment to find out the right radius!)
Lets select (click) the middle coin and press "D".
<br>
<img src="https://dl.dropboxusercontent.com/u/105935968/CoinWars/Snapshots/Merge1.png" alt="orignal"> 
<br>
Now we see tentative dotted lines indicating all coins within merge radius--two coins in this case. Next we select one of these 2 coins to continue specifying which coins we would like to merge...Let's select the top left coin and press "D" of course:
<br>
<img src="https://dl.dropboxusercontent.com/u/105935968/CoinWars/Snapshots/Merge2.png" alt="orignal"> 
<br>
Now a dotted line from the first has become solid, a confirmation of intent to merge, and a new dotted line emerges indicating what coin is reachable from this second coin. To finalize the merge, click the third coin (and press "D" of course):
<br>
<img src="https://dl.dropboxusercontent.com/u/105935968/CoinWars/Snapshots/Merge3.png" alt="orignal"> 
<br>
Almost there! Press "D" one last time to confirm the merge.
<br>
<img src="https://dl.dropboxusercontent.com/u/105935968/CoinWars/Snapshots/Merge4.png" alt="orignal"> 
<br>
And the merge is complete! Note that the health of this big coin is the average of healths from the smaller coins. Also, a newly merged coin does not have any powerups upon instantiation...

**Splitting:**

Whereas merging took 4 "D"'s, splitting only takes 1! First find an unlocked big coin, say this one below:
<br>
<img src="https://dl.dropboxusercontent.com/u/105935968/CoinWars/Snapshots/Split1.png" alt="orignal"> 
<br>
Click to select it:
<br>
<img src="https://dl.dropboxusercontent.com/u/105935968/CoinWars/Snapshots/Split2.png" alt="orignal"> 
<br>
Now press "D" to perform the split:
<br>
<img src="https://dl.dropboxusercontent.com/u/105935968/CoinWars/Snapshots/Split3.png" alt="orignal"> 
<br>
Several things to note...First, the 3 children are in a locked state. Can't move for this turn, greyed out. But on the other hand, the powerup that the big coin had is **preserved** Note that all the 3 small coins have the purple Dynamite powerup as indicated by their halos. Finally, their health is the same of the parent.

**Powerups: Dynamite, Fireball, Mirror** Each has interesting effects when indicent coin picks it up
<br>
<img src="https://dl.dropboxusercontent.com/u/105935968/CoinWars/Snapshots/Powerups.png" alt="orignal"> 
<br>
These rotating cubes in the game are the powerups; coins activate/acquire them when they pass through them. Coins that pick these up have their health halo's color change to the color of the powerup <br>
Dynamite: Coin equipped with this reduces the health of the next coin it comes into contact with by 33%
Fireball: Coin equipped with this imparts a powerful impulse to the next coin it comes into contact with
Mirror: Duplicates the coin going through it. Powerups are preserved.
<br>
**Player territories** Each turn players get more pipes (currently set to 2) that they can use to expand their territories. Territories are used for teleporting (see demo videos for me abusing their power!)
<br>
I am especially proud of the coin's 2-click launch mechanism that I came up with!<br>
To fire a coin, you need to 2 clicks, say at point A and at point B.  Now imagine a vector/line segment from A to B, AB.  If this vector "hits" a coin collider **first** (no other colliders) at say position C, then that coin will be launched in the direction AB and with force proportional to the ratio CB/AB.
<br>
This has some sweet properties, especially flexibility.  Say you clicked at point A but want to "break out" of the launch.  Then simply click somewhere else such that AB doesn't cross through a coin.  Then the launching state resets. Also, say you want to launch a coin really powerfully.  Then the obvious thing would be to place A far behind the coin and B really close to contact point(C).  But if you feel your first click at A wasn't far back enough, you don't have to break out--instead with practice, you can compensate by having your second B click be closer--due to the ratio process.  



Expand your territory--connected components can be used as portals (using "S" key) <-migrate this to "Gameplay"
