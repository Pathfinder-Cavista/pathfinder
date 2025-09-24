package com.example.cavistahackerthon_rt.routes

import kotlinx.serialization.Serializable

object AppRoutes {

    object Route {
        const val WELCOME ="welcome"
        const val LOGIN = "login"
        const val SIGNUP = "SignUp"
    }
    @Serializable
    sealed class Screen(val route: String) {
        @Serializable
        object Welcome: Screen(Route.WELCOME)
        @Serializable
        object Login: Screen(Route.LOGIN)
        @Serializable
        object SignUp: Screen(Route.SIGNUP)
    }
}