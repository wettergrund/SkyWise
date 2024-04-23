// Import the functions you need from the SDKs you need
import { initializeApp, getApps, getApp, deleteApp } from "firebase/app";
import { getAuth, setPersistence, inMemoryPersistence } from "firebase/auth"
// TODO: Add SDKs for Firebase products that you want to use
// https://firebase.google.com/docs/web/setup#available-libraries
const env = import.meta.env;
// Your web app's Firebase configuration
const firebaseConfig = {
  apiKey: env.VITE_APIKEY,
  authDomain: env.VITE_AUTHDOMAIN,
  projectId: env.VITE_PROJECTID,
  storageBucket: env.VITE_STORAGEBUCKET,
  messagingSenderId: env.VITE_MESSAGINGSENDERID,
  appId: env.VITE_APPID
};

// Initialize Firebase
let firebaseApp;
if (!getApps().length)
{
  firebaseApp = initializeApp(firebaseConfig);
}
else
{
  firebaseApp = getApp();
  deleteApp(firebaseApp);
  firebaseApp = initializeApp(firebaseConfig);
}

export const auth = getAuth(firebaseApp)
