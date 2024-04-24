<script>
    import Auth from '../components/Auth.svelte';
import { from, to } from '../stores/store'
import { authStore, authHandlers } from '../stores/authStore';
import { onMount } from 'svelte';
import { auth, handleG } from '../lib/firebase/firebase.client';
	import { browser } from '$app/environment';

	
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
<style lang="scss">
  @import '../style.scss';
  

  div {
    background-color:#cddefa;
  }
</style>
<div>


<h1>SkyWise</h1>


<p>{$from.toUpperCase()}</p>
<p>{$to.toUpperCase()}</p>

<p>Navbar</p>

</div>
<slot></slot>
Hej

{#if !$authStore.currentUser}
<Auth/>
<br>
<button on:click={handleG}>Google</button>
{:else}
{
  $authStore.currentUser.email

} <br>

<button on:click={async () => await authHandlers.logout()}>Logout</button>
{/if}

