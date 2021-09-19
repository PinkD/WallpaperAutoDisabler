# Wallpaper Auto Disabler

This program is designed to disable wallpaper switching when you play game
Because wallpaper switching will cause game stuck for about one second which is unacceptable

## Usage

### Auto Mode

This program will try to disable your wallpaper when it detects fullscreen app automatically

Default delay is 5000ms and you can modify it in app conf, the key is `delay`, value is number in ms

### Shortcut Mode

This program will toggle between enable and disable your wallpaper when you hit the shortcut

Default shortcut is `ctrl+alt+g` and you can modify it in app conf, the key is `delay`, value is `m_key1+m_key2+...+key` (m_keys are: `ctrl alt win shift`)

> NOTE: You'd better exit when you exit game because it regard some other fullscreen application as game too 
> The api is [SHQueryUserNotificationState](https://docs.microsoft.com/en-us/windows/win32/api/shellapi/nf-shellapi-shqueryusernotificationstate)

## Thanks And Dependencies

- [wpf-notifyicon](https://github.com/hardcodet/wpf-notifyicon)
- [NLog](https://github.com/NLog/NLog)

## License

```license
   Copyright 2021 PinkD

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
```
