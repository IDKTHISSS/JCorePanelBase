
# JCorePanelBase

This is the base for the panel and you can also make your own plugin based on this.


## Small Documentation


#### Steam
RunSteamWithParams - with this function you can start and logging to steam client
```
  Steam.RunSteamWithParams(JCSteamAccountInstance, string);
```
| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `account` | `JCSteamAccountInstance` | **Required**. Account to run steam |
| `Params` | `string` | Steam launch parameters|

#### GlobalMenager

```
  GlobalMenager.ShowDialog(string);
```
ShowDialog - show dialog in main window with your messege
| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `messege`      | `string` | **Required**. Messege to display |

```
  GlobalMenager.ShowInput(string, string, (string) => { HERE YOUR CODE });
```

ShowInput - show dialog in main window with your messege and user can input text
| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `messege`      | `string` | **Required**. Messege to display |
| `placeholder`      | `string` | **Required**. Placeholder in InputBox |
| `callback`      | `Action<string>` | **Required**. This is was called after user close window in string you get answer|

```
  GlobalMenager.ShowConfirm(string, string, (bool) => { HERE YOUR CODE });
```

ShowConfirm - show dialog in main window with your messege and user can confirm some actions
| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `messege`      | `string` | **Required**. Messege to display |
| `callback`      | `Action<bool>` | **Required**. This is was called after user close window in bool you get answer|

```
  GlobalMenager.GetProperty(string);
```

GetProperty - return you value of property by property name
| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `PropertyName`      | `string` | **Required**. Property Name |

```
  GlobalMenager.GetSteamPath();
```

GetSteamPath - return you Steam path



## License

[MIT](https://choosealicense.com/licenses/mit/)

