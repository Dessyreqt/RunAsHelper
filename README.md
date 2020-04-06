# RunAsHelper
A little helper utility to manage multiple runas credentials for multiple applications

## Why does this exist?
At my day job, we have multiple domains for our environments on which we need to run things like SQL Server, or various build scripts. The simple implementation is simply to have a batch file which points to SQL Server and tells it to runas the domain user for that environment. Everytime the window pops up, you have to type in your password and hit enter. Normally this wouldn't be an issue, but these passwords are very long, computer generated, and only last for a day or so.

This little helper will cache the passwords and the applications on which runas is invoked. The information is stored in  `%LOCALAPPDATA%\RunAsHelper`. The runas command is invoked with the -netonly flag.

## How do I use this?

The Username box is editable. After adding in a username, any password entered will be automatically saved to that username so that when the username is selected from the dropdown, the password will repopulate.

The Application field works in much the same way with the Path box. One thing to note about the path, enter it as you would on the command line. If the path would require quotes on the command line, add the quotes to the text box. Simply put, you should be able to open up a shortcut to your application (say Visual Studio) and copy the path used, then put it directly into the path box on the RunAsHelper.

When you click "Copy Password and Run", the password (along with a newline character) will be put onto the clipboard, and the appropriate runas command will be issued. You will need to type in your password, which is conveniently on the clipboard now. Simply pressing Ctrl+V should paste in the password and start the application.

## What features need to be added to this?

This is basically a short little helper application but a few things that can be done to make it a little more usable for more people. Here are some things I have in mind:

- Make the password able to toggle hidden/visible.
- Make "netonly" optional
- Make appending the newline optional
- Optionally clear the clipboard a few seconds after clicking the "Copy Password and Run" button.
