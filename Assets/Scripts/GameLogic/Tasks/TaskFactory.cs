using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TaskFactory {
    public static List<ITask> DefaultList =>
        new List<ITask>() {
            new DeployableTask("Client Searching API", 20, new List<Feature>() {
                new Feature("Search by location / destination", 300, 0.26f),
                new Feature("Search by check in date", 700, 0.24f),
                new Feature("Search by check out date", 700, 0.31f)
            }),
            new DeployableTask("Client Accommodation API", 30, new List<Feature>() {
                new Feature("Add check in date", 800, 0.4f),
                new Feature("Add check out date", 800, 0.3f),
                new Feature("Add number of guests", 900, 0.3f)
            }),
            new DeployableTask("Client Reservation API", 35, new List<Feature>() {
                new Feature("Add status for reservation request ", 300, 0.5f),
                new Feature("Make changes to reservation request", 1300, 0.32f)
            }),
            new DeployableTask("User Preference API", 45, new List<Feature>() {
                new Feature("Save or remove rental homes", 1600, 0.43f),
                new Feature("Create new list or add to existing list", 1700, 0.5f),
                new Feature("Generate invitation link for editing and viewing", 1900, 0.59f)
            }),
            new DeployableTask("Trip Planning API", 60, new List<Feature>() {
                new Feature("Invite friends to plan trip by email", 2500, 0.62f),
                new Feature("Generate plan invitation link", 2700, 0.62f)
            }),
            new DeployableTask("Payment API", 180, new List<Feature>() {
                new Feature("Save new payment method", 2800, 0.71f),
                new Feature("Edit payment method", 3500, 0.75f),
                new Feature("Remove payment method", 4100, 0.76f)
            }),
            new DeployableTask("Host Accommodation API (1)", 140, new List<Feature>() {
                new Feature("Search by location / destination", 3300, 0.83f),
                new Feature("Search by check in date", 3300, 0.84f)
            }),
            new DeployableTask("Host Accommodation API (2)", 300, new List<Feature>() {
                new Feature("Add co-host via invitation link", 2700, 0.86f),
                new Feature("Add co-host via email address", 3700, 0.91f),
                new Feature("Set co-host permissions", 4700, 0.92f)
            }),
            new DeployableTask("Host Dashboard", 200, new List<Feature>() {
                new Feature("Show recommendation and statistics", 4800, 0.93f),
                new Feature("Show search results of guests in host’s area", 4600, 0.71f)
            }),
            new DeployableTask("In-app messaging (1)", 100, new List<Feature>() {
                new Feature("Add bot for customer support", 5000, 0.97f),
                new Feature("Link to booking details", 300, 0.29f),
                new Feature("Send kindness card or financial contribution to host", 700, 0.23f)
            }),
            new DeployableTask("In-app messaging (2)", 60, new List<Feature>() {
                new Feature("Change font size", 600, 0.21f),
                new Feature("Notification", 1000, 0.3f),
                new Feature("Report message", 2100, 0.43f)
            })
        };
}