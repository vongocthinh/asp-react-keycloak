import { useMemo, useCallback } from "react";
import KeyCloakService from "./services/KeycloakService";
import HttpService from "./services/HttpService";

function App() {
  const userRoles = useMemo(() => KeyCloakService.GetUserRoles(), []);
  const logout = useCallback(() => KeyCloakService.CallLogout() ,[]);
  const fetchWeatherForcase = useCallback(() => HttpService.getAxiosClient()
    .get("https://localhost:7056/WeatherForecast")
    .then(
      (p) => alert(JSON.stringify(p.data)),
      (e) => {
        console.log(e);
        alert(e.message)
      }
    ), []
  )

  return (
    <div>
      <p>Welcome: {KeyCloakService.GetUserName()}</p>
      {userRoles && <div>Role: {userRoles.join(', ')}</div>}
      <button onClick={logout}>Logout</button>
      <button onClick={fetchWeatherForcase}>WeatherCast</button>
    </div>
  )
}

export default App;
