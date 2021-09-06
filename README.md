#A half-finished [NFive](https://github.com/NFive/NFive) core for FiveM
This was a project I started which was designed to be a reimplementation of GTA:Online in FiveM.

I have since lost interest in continuing, but I'm hoping the code could be useful for other people to use, especially the scaleform phone.

This isn't designed to be cloned and used, it's moreso for the code snippets.

## Scaleform Phone
One of (in my opinion) the most lacking things in FiveM was a good scaleform phone that would have the same feeling as that in the base game. KGV-Phone was great but LUA based, which I detest, so I took some time to make it in C#, inspired by KGV-Phone and [this post](https://forum.cfx.re/t/sourcecode-release-ifruit-phone-in-c/777054).

To make it work you need to **stream the edited(important! the regular will not work!) gfx file** from `Core.Client/resources/phone` and alter the code in `Core.Client/Phone` to fit to whatever framework you are using.

I don't remember much of how I left off, but here is a **tentative (features may be incomplete/not working 100%)** list of what is implemented in the phone:
- Fully scaleform based
- Homescreen with scroll/keyboard inputs to navigate
- Apps encapsulated into their own classes to easily organize data:
  - Contacts
  - Camera
  - Messages
  - Viewing text messages
  - Calling contacts screen
  - Failed attempt at getting the eyefind scaleform to work
  - Settings app with working themes
