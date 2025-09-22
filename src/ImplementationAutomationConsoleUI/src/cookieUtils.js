import VueCookies from 'vue-cookies';

export function extendCookieTimeout() {
  const newExpiryTimeInSeconds = 1800;
  VueCookies.set("cookie", VueCookies.get("cookie"), newExpiryTimeInSeconds);
  VueCookies.set("enc", VueCookies.get("enc"), newExpiryTimeInSeconds);
  VueCookies.set("encVal", VueCookies.get("encVal"), newExpiryTimeInSeconds);
  VueCookies.set("userName", VueCookies.get("userName"), newExpiryTimeInSeconds);
  VueCookies.set("userTypeCode", VueCookies.get("userTypeCode"), newExpiryTimeInSeconds);
  VueCookies.set("name", VueCookies.get("name"), newExpiryTimeInSeconds);
}