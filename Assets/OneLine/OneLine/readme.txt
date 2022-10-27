# Overview

OneLine improves your databases and makes it more readable for people. It draws 
objects in Inspector into one line instead default line-by-line style. Also it 
provides a few features like fields highlightning, locking array size, etc...

# Gettings Started

    - After importing look at Assets/Example/Overview/Overview.asset and open it 
in the Inspector Window. It will show you all capabilities of OneLine library.

    - In your code, add `using OneLine;` and add `[OneLine]` to fields you want 
to draw into one line. Note that internal fields don't need to be marked with 
`[OneLine]`: they will be processed automatically.

    - If you want to customize linified fields, use `[Width]`, `[Weight]`, 
`[HideLabel]`, `[Highlight]`, `[HideButtons]` and `[ArrayLength]` attributes 
(For details look at Overview.asset).

    - To separate lines use `[Separator]` instead built-in `[Header]`. 
`[Separator]` is just a nice-looked [Header].

    - To follow object references in inspector use `[Expandable]` and 
`[ReadOnlyExpandable]`. 

# RectEx

One more thing: RectEx.

RectEx (rect extensions) is the small lighweight library which makes your work
with EditorGUI easily. It provides some extension methods for class Rect.

For example:
    - rect.Column(5) -- slices rect for 5 equal pieces with horizontal lines;
    - rect.Row(5) -- same as Column, but use vertical lines;
    - rect.Grid(2,3) -- return Rect[2,3];
    - ...
    - a lot of them

For details go to https://github.com/slavniyteo/rect-ex

# Contributing

For more information and support go to https://github.com/slavniyteo/one-line 

