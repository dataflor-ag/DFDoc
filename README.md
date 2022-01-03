# DATAflor DFDoc
Formatbeschreibung DATAflor Import-Dokument


Dieses Projekt enthält die Formatbeschreibung für das DFDoc-Format, das von DATAflor BUSINESS zum Import in die Datenbank verwendet wird.
Es enthält eine .net-standard 2.0 kompatible Assembly DATAflor.Dfdoc zum Erzeugen und Lesen von DFDoc-Dateien.

Die Schema-Datei für die eingebettete XML-Datei befindet als [DFDoc.xsd](src/DATAflor.Dfdoc/Schema/DFDoc.xsd) im Verzeichnis Schema im Projekt DATAflor.Dfdoc.

## Dateiformat

Bei einer DFDoc-Datei handelt es sich um eine ZIP-kompatible Datei mit der Endung **.dfdoc**.
Innerhalb dieses Archivs muss es eine Datei mit dem Name **Metadata.xml** geben.

In der Datei Metadata.xml sind die Metadaten zu den Dateien aufgeführt, die in das DATAflor BUSINESS importiert werden sollen.
Zu jeder Datei in dem Archiv (außer der Metadaten.xml) muss es einen entsprechenden Datenblock in der Metadatendatei geben.

### Aufbau Metadaten.xml

Bei der Datei Metadaten.xml handelt es sich um eine XML-Datei mit dem Root-Knoten "Dokumente". 
Innerhalb des Rootknotens ist das erste Element der Eintrag "Version". Dieses Feld gibt die Format-Version der Datei an und muss zum gegenwärtigen Zeitpunkt 1 sein.
Darauf folgt ein optionales Element "Programmsystem" das Informationen zum Programm enthält, dass die Datei erzeugt hat.
Anschließend kommt eine Liste von "Dokument"-Einträgen. Jeder Dokument-Eintrag beschreibt eine Datei im Archiv.

Die Schema-Datei kann [hier](/docs/DFDoc.png) als Grafik betrachtet werden.

### Aufbau Dokument-Element

Der erste Eintrag in dem Dokument-Eintrag ist das Element "Dateiname". Dieses Feld enthält den Namen der korrespondierenden Datendatei 
(die in das DATAflor BUSINESS importiert werden soll). Sind die Dateien im Archiv in Unterverzeichnissen organisiert, muss auch der Dateiname die entsprechenden Pfadangaben enthalten.

Danach folgt das Element "Ziel". Der Inhalt dieses Element bestimmt zum Einen welche Aktion beim Import in das DATAflor BUSINESS ausgeführt wird und zum Anderen
welche Metadaten für das Dokument erwartet werden. In der gegenwärtigen Version 1 wird nur das Ziel "DMS" unterstützt.

Im Anschluss an das Ziel-Element kommt das optionale "Ersteller"-Element, das Informationen zum Ersteller der jeweiligen Datei enthalten kann.

Den Abschluss des Inhaltes des Dokument-Elementes bilden die jeweiligen Metadaten zu dem aktuellen Dokument. In Version 1 wird dort nur das Element "[DMS](docs/DMSElement.md)" unterstützt.

Eine DFDoc-Beispieldatei mit zwei Dateien zufälligen Inhalts findet sich unter [test.dfdoc](samples/test.dfdoc).

### Zuordnung Ziel-Metadatenelement

Die folgende Tabelle gibt an für welches Ziel welches Metadaten-Element erwartet wird.

| Ziel | Metadaten-Element |
| ---- | ----------------- |
| DMS  | [DMS](docs/DMSElement.md) |

## Utility DATAflor.Dfdoc.Tool

Mit dem Tool DATAflor.Dfdoc.Tool.exe kann eine existierende DFDoc-Datei entpackt werden. Damit kann also die syntaktische Korrektheit der Datei geprüft werden.

Aufruf: DATAflor.Dfdoc.Tool.exe "[DFDoc]" "[Dir]".

[DFDoc] Name und Pfad zur DFDoc-Datei
 
[Dir] Existierendes Verzeichnis in das die Dateien aus der übergebenen DFDoc-Datei entpackt werden.

## Versionshistorie

| Datum | Version | Anmerkungen |
| ---- | --- | --- |
| 2020.11.30  | 1 | Initiale Version. Unterstützung DMS-Import|


