<script>
import { authStore, authHandlers } from '../stores/authStore';
import { Button, Modal, Label, Input, Checkbox, NavLi } from 'flowbite-svelte';
import {  ArrowRightToBracketOutline  } from 'flowbite-svelte-icons';

	import Auth from './Auth.svelte';

</script>


{#if !$authStore.currentUser}
<Auth/>
{:else}

<!-- <Button outline color="dark" size="xs" on:click={async () => await authHandlers.logout()}>
    
    </Button> -->
    <NavLi href="#" on:click={async () => await authHandlers.logout()}>Logout</NavLi>
{/if}