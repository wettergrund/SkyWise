<script>
    import Auth from '../components/Auth.svelte';
import { from, to } from '../stores/store'
import { authStore, authHandlers } from '../stores/authStore';
import { onMount } from 'svelte';
import { auth, handleG } from '../lib/firebase/firebase.client';
	import { browser } from '$app/environment';

	import Login from '../components/Login.svelte';
	import Navbar from '../components/Navbar.svelte';

import "../app.pcss"
import "../style.scss"
	  import { Footer, FooterBrand, FooterCopyright, FooterIcon, FooterLink, FooterLinkGroup } from 'flowbite-svelte';
onMount(() => {
		const unsubscribe = auth.onAuthStateChanged((user) => {
			console.log(user);
			// @ts-ignore
			authStore.update((curr) => {
				return { ...curr, isLoading: false, currentUser: user };
			});

			if (
				browser &&
				!$authStore?.currentUser &&
				!$authStore.isLoading &&
				window.location.pathname !== '/'
			) {
				// window.location.href = '/';
				// @ts-ignore
				console.log(authStore.currentUser, authStore.isLoading);
			}
		});


		return unsubscribe;
	});


  // your script goes here
</script>
<Navbar/>


<div style="min-height: 80vh;">




<slot></slot>
</div>
<Footer>© Copy</Footer>
