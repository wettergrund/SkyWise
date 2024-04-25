<script>
    import { Navbar, NavBrand ,NavHamburger, NavUl, NavLi} from 'flowbite-svelte';
	import Login from './Login.svelte';
 import { page } from '$app/stores'
$: activeUrl = $page.url.pathname;
</script>

<Navbar  >
    <NavBrand href="/">
      <!-- <img src="/images/flowbite-svelte-icon-logo.svg" class="me-3 h-6 sm:h-9" alt="Flowbite Logo" /> -->
      <span class="self-center whitespace-nowrap text-xl font-semibold dark:text-white">SkyWise</span>
    </NavBrand>
    <NavHamburger  />
    <NavUl {activeUrl} >
      <NavLi href="/">Home</NavLi>
      <NavLi href="/search">Weather</NavLi>
      <Login/>

    </NavUl>
  </Navbar>

