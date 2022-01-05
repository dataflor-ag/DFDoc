### Beschreibung Element DMS
 
Die Verwendung des Ziel "DMS" bedingt das Vorhandensein eines Elementes "DMS" in dem entsprechenden Dokument-Element.

Aufbau des DMS-Element:

- Bezeichnung : Bezeichnung des Dokumentes im DMS
- Verantwortlich : Verantwortlicher im DMS
- Kategorie : Kategorie für den DMS-Eintrag
- Dokumentenart : Dokumentenart für den DMS-Eintrag
- Notiz : Notiz/Schlagworte
- Hauptrefenz : Element vom Typ ReferenzeType. Legt den Datensatz fest, an dem das Dokument primär "hängt".
- WeitereReferenzen : optionale Liste mit Elementen vom Typ WeitereReferenz für Verweise auf das Dokument zusätzlich zur Hauptreferenz.


Die Elemente Bezeichnung, Verantwort, Kategorie und Dokumentenart sollten mit einem Wert gefüllt sein, dass sich auch in der jeweiligen Liste im Dokumenteninformationsdialog findet. Andernfalls könnte es zu Problemen kommen, wenn nach dem Dokument gesucht wird.

Notiz ist ein Klartext ohne jegliche Formatierung. 

Hauptreferenz und WeitereReferenzen sollten auf existierende Datensätze im DATAflor BUSINESS verweisen. Sonst kann das Dokument evtl. nur über die Suche gefunden werden, da es keinem bekannten Datensatz zugeordnet ist.

## Verknüpfungen des Dokumentes im DATAflor BUSINESS

Um das importierte Dokument mit vorhandenen Datensätzen im DATAflor BUSINESS zu verknüpfen sind die Einträge HauptReferenz und WeitereRefenzen wichtig.
Über diese Element wird das Dokument mit einem Datensatz im DATAflor BUSINESS verknüpft. Die Hauptreferenz ist ein Mussfeld, damit das Dokument mindestens an einem Datensatz hängt.
Alle übergeordneten Datensätze zu einem übergebenen Datensatz (z.B. bei einer Positions-Id die Id des LVs, des Projekts und des Auftraggebers) werden vom Import selbst ermittelt und müssen in den Metadaten nicht angegeben werden.

### Beschreibung des Typs ReferenceType

Der Typ ReferenceType ist die Definition der Elemente Hauptreferenz und WeitereReferenz.

Er besteht aus den folgenden Elementen:

- RefTyp : Gibt an auf welche Art von Datensatz sich die folgende RefId bezieht.
- RefId : Die Id mit dem das Dokument im DATAflor BUSINESS verknüpft werden soll.

Für das Element RefTyp sind folgende Werte zulässig:

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
| GERÄT | Id bezeichnet ein Gerät |
| LEISTUNG | Id bezeichnet eine Standardleistung |
| KATALOG | Id bezeichnet einen Standardleistungskatalog |
| LV-NR | Id bezeichnet eine LV-Nummer |
| PERSONAL-NR | Id bezeichnet eine Personalnummer |
| GERAETE-NR | Id bezeichnet eine Gerätenummer |
