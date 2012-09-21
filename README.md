schedule-metro
==============

A ModernUI-style class schedule app, written in C#, WPF.

Build
-----

**This is a development version, which may or may not compile and/or work.**

Built using Visual Studio 2012 Professional with NuGet package manager extension. Not tested on other configurations.

Targeted for .NET Framework 4.0

Schedule file
-------------

The schedule is an .xml file, which describes:

* Class types (regular class, lab, etc.);
* Student groups (if the schedule is different for some groups of students);
* All available classes;
* The schedule itself.

The .xml file is currently loaded from a harcoded location in the internet.

An example of a valid .xml file is in the folder `Schedule`.