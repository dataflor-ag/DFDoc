### Beschreibung Element DMS
 
Die Verwendung des Ziel "DMS" bedingt das Vorhandensein eines Elementes "DMS" in dem entsprechenden Dokument-Element.

Aufbau des DMS-Element:

- Bezeichnung : Bezeichnung des Dokumentes im DMS
- Verantwortlich : Verantwortlicher im DMS
- Kategorie : Kategorie f�r den DMS-Eintrag
- Dokumentenart : Dokumentenart f�r den DMS-Eintrag
- Notiz : Notiz/Schlagworte
- Hauptrefenz : Element vom Typ ReferenzeType. Legt den Datensatz fest, an dem das Dokument prim�r "h�ngt".
- WeitereReferenzen : optionale Liste mit Elementen vom Typ WeitereReferenz f�r Verweise auf das Dokument zus�tzlich zur Hauptreferenz.


Die Elemente Bezeichnung, Verantwort, Kategorie und Dokumentenart sollten mit einem Wert gef�llt sein, dass sich auch in der jeweiligen Liste im Dokumenteninformationsdialog findet. Andernfalls k�nnte es zu Problemen kommen, wenn nach dem Dokument gesucht wird.

Notiz ist ein Klartext ohne jegliche Formatierung. 

Hauptreferenz und WeitereReferenzen sollten auf existierende Datens�tze im DATAflor BUSINESS verweisen. Sonst kann das Dokument evtl. nur �ber die Suche gefunden werden, da es keinem bekannten Datensatz zugeordnet ist.

## Verkn�pfungen des Dokumentes im DATAflor BUSINESS

Um das importierte Dokument mit vorhandenen Datens�tzen im DATAflor BUSINESS zu verkn�pfen sind die Eintr�ge HauptReferenz und WeitereRefenzen wichtig.
�ber diese Element wird das Dokument mit einem Datensatz im DATAflor BUSINESS verkn�pft. Die Hauptreferenz ist ein Mussfeld, damit das Dokument mindestens an einem Datensatz h�ngt.
Alle �bergeordneten Datens�tze zu einem �bergebenen Datensatz (z.B. bei einer Positions-Id die Id des LVs, des Projekts und des Auftraggebers) werden vom Import selbst ermittelt und m�ssen in den Metadaten nicht angegeben werden.

### Beschreibung des Typs ReferenceType

Der Typ ReferenceType ist die Definition der Elemente Hauptreferenz und WeitereReferenz.

Er besteht aus den folgenden Elementen:

- RefTyp : Gibt an auf welche Art von Datensatz sich die folgende RefId bezieht.
- RefId : Die Id mit dem das Dokument im DATAflor BUSINESS verkn�pft werden soll.

F�r das Element RefTyp sind folgende Werte zul�ssig:

| RefTyp | Art der Datensatz-Id |
| --- | --- |
| ADRESSE | Id bezeichnet eine Adresse |
| AUFTRAGGEBER | Id bezeichnet einen Auftraggeber in der Projektverwaltung |
| KONTAKT | Id bezeichnet einen Kontakt einer Adresse |
| MITARBEITER | Id bezeichnet einen Mitarbeiter |
| LV | Id bezeichnet ein LV |
| LV-VARIANTE | Id bezeichnet ein Angebotsvariante |
| LV-NACHTRAG | Id bezeichnet ein Nachtragsangebot |
| LV-BIETER | Id bezeichnet ein Bieter-LV |
| PROJEKT | Id bezeichnet ein Projekt |
| OBJEKT | Id bezeichnet ein Objekt |
| POSITION | Id bezeichnet eine Position |
| HIERACHIESTUFE | Id bezeichnet eine Hierarchiestufe in einem LV |
| GER�T | Id bezeichnet ein Ger�t |
| LEISTUNG | Id bezeichnet eine Standardleistung |
| KATALOG | Id bezeichnet einen Standardleistungskatalog |
