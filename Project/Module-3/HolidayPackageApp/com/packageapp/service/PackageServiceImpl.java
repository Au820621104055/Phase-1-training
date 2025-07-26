package com.packageapp.service;

import java.util.List;
import com.packageapp.dao.PackageDao;
import com.packageapp.dao.PackageDaoImpl;
import com.packageapp.model.Package;
import com.packageapp.exception.InvalidPackageIdException;

public class PackageServiceImpl implements PackageService {
    private PackageDao dao = new PackageDaoImpl();

    private boolean validatePackageId(String id) {
        return id != null && id.length() == 7;
    }

    @Override
    public void addPackage(Package pkg) throws InvalidPackageIdException {
        if (!validatePackageId(pkg.getPackageId())) {
            throw new InvalidPackageIdException("Invalid Package Id");
        }
        dao.addPackage(pkg);
    }

    @Override
    public List<Package> fetchAllPackages() {
        return dao.getAllPackages();
    }

    @Override
    public Package findPackageById(String id) {
        return dao.getPackageById(id);
    }

    @Override
    public void calculateCost(String id) throws InvalidPackageIdException {
        if (!validatePackageId(id)) {
            throw new InvalidPackageIdException("Invalid Package Id");
        }
        Package pkg = dao.getPackageById(id);
        if (pkg != null) {
            dao.calculatePackageCost(pkg);
        } else {
            System.out.println("Package not found.");
        }
    }
}