const CryptoJS = require("crypto-js");

export default class EncryptionService {
  constructor(context) {
    const hash = CryptoJS.SHA256(context.secretKey);
    this.key = CryptoJS.enc.Hex.parse(hash.toString());
    this.iv = CryptoJS.lib.WordArray.create(this.key.words.slice(0, 4));
  }

  encrypt(data) {
    const encrypted = CryptoJS.AES.encrypt(data, this.key, {
      iv: this.iv,
      padding: CryptoJS.pad.Pkcs7,
      mode: CryptoJS.mode.CBC,
    });
    return encrypted.toString();
  }

  decrypt(data) {
    const decrypted = CryptoJS.AES.decrypt(data, this.key, {
      iv: this.iv,
      padding: CryptoJS.pad.Pkcs7,
      mode: CryptoJS.mode.CBC,
    });
    return decrypted.toString(CryptoJS.enc.Utf8);
  }
}
