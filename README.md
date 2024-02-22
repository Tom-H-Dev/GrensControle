# Code Convension
Private variables:     _privateVariables  
Public variables:    _publicVariables  
Protected variables:    p_protectedVariables  
Local variables:    l_localVariables  
Class names:        ClassName()  

# GrensControle
 
elke speler heeft een apart gebied om te interacteren met het voertuig:

Speler 1 (GateGuard): Interact met de knop om de persoon door te laten of aan te              houden.
Speler 2 (QuestGuard): Interact met de driver en kan hem ondervragen.
Speler 3 (SearchGuard): Interact met het voertuig.


## Speler 1:
Het einddoel van speler 1 (GateGuard) is om te beoordelen of de persoon in de auto door zou mogen of niet. Dit doet speler 1 door met speler 2 (QuestGuard) en speler 3 (Searchguard) te communiceren om verdachte situaties vast te stellen en hieruit dus een oordeel halen. Communicatie zou kunnen via een proximity-chat of via de UI. Speler 1 checkt ook naar papieren die hij krijgt van speler 2. In deze papieren staat wat informatie over de persoon in de auto. Het doel is hier om te checken of de persoon in de auto er überhaupt door mag zonder te kijken naar verdacht gedrag of voorwerpen in zijn auto. Wanneer speler 2 of 3 iets verdachts meemaken dan kunnen zij dat doorgeven aan speler 1 door middel van een melding die ze kunnen maken in een menuutje. Die meldingen kan speler 1 dan weer zien.



## Speler 2:
Het doel van speler 2 is de persoon in het voertuig te ondervragen en erachter komen of het een verdachte situatie is. Als speler 2 naar het voertuig toe loopt dan verschijnt er een interactie text die zegt dat speler 2 op “E” kan drukken om een interactie menu tevoorschijn te halen. In dit scherm krijgt speler 2-3 vragen waaruit hij kan kiezen en de persoon in het voertuig reageert hierop. Als speler 2 een vraag stelt en de persoon in de auto reageert hierop, verschijnen er elke keer weer nieuwe vragen. Soms kan het zijn dat je een paar keer dezelfde vraag kan stellen. Uit deze reacties en handelingen van de persoon uit de auto kan speler 2 vaststellen of het een verdachte situatie is. Als speler 2 vaker dezelfde vraag stelt, kan persoon in de auto lastiger reageren. Als het een verdachte situatie mocht zijn dan kan speler 2 dit doorgeven aan speler 1 door middel van een melding die speler 2 kan maken in een menuutje.

Speler 2 vragen:
- Mag ik uw papieren zien?
- Heeft u wapens in uw voertuig?
- Wat komt u doen?
- Wie bent u?






Reacties van persoon:
Vrolijk: Reactie is wat milder en werkt sneller mee. Is moeilijk boos te krijgen.
Neutraal: Reactie is wat milder, maar heeft ook geen zin om lang te wachten.
Chagrijnig: Reactie kan sneller geïrriteerd en werkt niet heel snel mee.

Zenuwachtig: Reactie is onduidelijk vanwege verdacht iets en spreekt zichzelf tegen.

## Speler 3:
Het doel van speler 3 is om de auto veilig te stellen van verdachte voorwerpen. Deze voorwerpen kan speler 3 vinden door om de auto heen te lopen, deuren te openen en te zoeken naar voorwerpen. Als speler 3 dichtbij een deur of klep komt dan verschijnt er in het scherm een UI beeld waar je dan “E” kan drukken om de deur of klep te openen. Hierin kunnen dan voorwerpen liggen. Dit verschilt en is compleet willekeurig met wat er ligt en of er überhaupt iets ligt. Wanneer speler 3 iets ligt wat verdacht is dan kan speler 3 dit doorgeven aan speler 1 door middel van een melding die speler 3 kan maken in een menuutje.

Gevonden voorwerpen:
- Vuurwapen 
- Steekwapen
- Munitie
- Rugtas
- Koffer
- Leeg

Nodige models:
Guard house met interieur
2 Knoppen
Clipboard of papieren
Guard model met animaties
	- Idle
	- Walking
	- Interacting
	- Crouch
 - Auto met animaties of apart delen van deuren
 Vuurwapen 
 Steekwapen
 Munitie
 Rugtas
 Koffer
 Leeg
